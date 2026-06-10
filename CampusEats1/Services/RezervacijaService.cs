using CampusEats.Models;
using CampusEats.Repositories;

namespace CampusEats.Services;

public class RezervacijaService : IRezervacijaService
{
    private readonly IAktivnostLogService _aktivnostLogService;
    private readonly IObrokRepository _obrokRepository;
    private readonly IObavijestService _obavijestService;
    private readonly IRezervacijaRepository _rezervacijaRepository;

    public RezervacijaService(
        IRezervacijaRepository rezervacijaRepository,
        IObrokRepository obrokRepository,
        IObavijestService obavijestService,
        IAktivnostLogService aktivnostLogService)
    {
        _rezervacijaRepository = rezervacijaRepository;
        _obrokRepository = obrokRepository;
        _obavijestService = obavijestService;
        _aktivnostLogService = aktivnostLogService;
    }

    public Task<List<Rezervacija>> GetAllAsync()
    {
        return _rezervacijaRepository.GetAllAsync();
    }

    public Task<List<Rezervacija>> GetByKorisnikIdAsync(int korisnikId)
    {
        return _rezervacijaRepository.GetByKorisnikIdAsync(korisnikId);
    }

    public Task<List<Rezervacija>> GetDeliveriesAsync()
    {
        return _rezervacijaRepository.GetDeliveriesAsync();
    }

    public Task<Rezervacija?> GetByIdAsync(int? id)
    {
        return id is null ? Task.FromResult<Rezervacija?>(null) : _rezervacijaRepository.GetByIdAsync(id.Value);
    }

    public Task<Rezervacija?> GetByIdWithDetailsAsync(int? id)
    {
        return id is null ? Task.FromResult<Rezervacija?>(null) : _rezervacijaRepository.GetByIdWithDetailsAsync(id.Value);
    }

    public async Task<(bool Success, string? Error)> CreateAsync(Rezervacija rezervacija)
    {
        var obrok = await _obrokRepository.GetByIdAsync(rezervacija.ObrokId);
        if (obrok is null || !obrok.Dostupan || obrok.Kolicina <= 0)
        {
            return (false, "Odabrani obrok trenutno nije dostupan.");
        }

        rezervacija.Datum = DateTime.Now;
        rezervacija.Status = StatusRezervacije.Potvrdjena;
        rezervacija.QRKod = new QRKod();
        obrok.Kolicina--;
        obrok.Dostupan = obrok.Kolicina > 0;

        await _rezervacijaRepository.AddAsync(rezervacija);
        await _rezervacijaRepository.SaveChangesAsync();
        await _obavijestService.KreirajAsync(
            rezervacija.KorisnikId,
            rezervacija.Id,
            "Rezervacija potvrdjena",
            $"Vasa rezervacija #{rezervacija.Id} je potvrdjena.");
        await _obavijestService.KreirajZaUloguAsync(
            UlogaKorisnika.RadnikMenze,
            rezervacija.Id,
            "Nova narudzba",
            $"Kreirana je nova rezervacija #{rezervacija.Id}.");

        if (rezervacija.NacinPreuzimanja == NacinPreuzimanja.Dostava)
        {
            await _obavijestService.KreirajZaUloguAsync(
                UlogaKorisnika.Kurir,
                rezervacija.Id,
                "Nova dostavna narudzba",
                $"Rezervacija #{rezervacija.Id} ceka kurira.");
        }

        await _aktivnostLogService.ZabiljeziAsync("Kreiranje rezervacije", nameof(Rezervacija), rezervacija.Id, $"Kreirana rezervacija #{rezervacija.Id}.");
        return (true, null);
    }

    public async Task<bool> UpdateAsync(int id, Rezervacija rezervacija)
    {
        if (id != rezervacija.Id)
        {
            return false;
        }

        _rezervacijaRepository.Update(rezervacija);
        await _rezervacijaRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateStatusAsync(int id, StatusRezervacije status)
    {
        var rezervacija = await _rezervacijaRepository.GetByIdAsync(id);
        if (rezervacija is null)
        {
            return false;
        }

        rezervacija.Status = status;
        await _rezervacijaRepository.SaveChangesAsync();
        await _obavijestService.KreirajAsync(
            rezervacija.KorisnikId,
            rezervacija.Id,
            "Promjena statusa",
            $"Status rezervacije #{rezervacija.Id} je promijenjen u {status}.");

        if (status == StatusRezervacije.Spremna && rezervacija.NacinPreuzimanja == NacinPreuzimanja.Dostava)
        {
            if (rezervacija.KurirId is not null)
            {
                await _obavijestService.KreirajAsync(
                    rezervacija.KurirId,
                    rezervacija.Id,
                    "Narudzba spremna",
                    $"Rezervacija #{rezervacija.Id} je spremna za dostavu.");
            }
            else
            {
                await _obavijestService.KreirajZaUloguAsync(
                    UlogaKorisnika.Kurir,
                    rezervacija.Id,
                    "Narudzba spremna",
                    $"Rezervacija #{rezervacija.Id} je spremna za preuzimanje kurira.");
            }
        }

        await _aktivnostLogService.ZabiljeziAsync("Promjena statusa", nameof(Rezervacija), rezervacija.Id, $"Status rezervacije promijenjen u {status}.");
        return true;
    }

    public async Task<bool> DodijeliKuriruAsync(int id, int kurirId)
    {
        var rezervacija = await _rezervacijaRepository.GetByIdAsync(id);
        if (rezervacija is null || rezervacija.NacinPreuzimanja != NacinPreuzimanja.Dostava)
        {
            return false;
        }

        if (rezervacija.KurirId is not null && rezervacija.KurirId != kurirId)
        {
            return false;
        }

        rezervacija.KurirId = kurirId;
        rezervacija.Status = StatusRezervacije.Spremna;
        await _rezervacijaRepository.SaveChangesAsync();
        await _obavijestService.KreirajAsync(
            rezervacija.KorisnikId,
            rezervacija.Id,
            "Kurir dodijeljen",
            $"Kurir je preuzeo obradu rezervacije #{rezervacija.Id}.");
        await _obavijestService.KreirajAsync(
            kurirId,
            rezervacija.Id,
            "Dostava preuzeta",
            $"Preuzeli ste rezervaciju #{rezervacija.Id} za dostavu.");
        await _aktivnostLogService.ZabiljeziAsync("Dodjela kuriru", nameof(Rezervacija), rezervacija.Id, $"Rezervacija dodijeljena kuriru #{kurirId}.");
        return true;
    }

    public async Task DeleteAsync(int id)
    {
        var rezervacija = await _rezervacijaRepository.GetByIdWithQrAsync(id);
        if (rezervacija is null)
        {
            return;
        }

        _rezervacijaRepository.Remove(rezervacija);
        await _rezervacijaRepository.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(int id)
    {
        return _rezervacijaRepository.ExistsAsync(id);
    }

    public Task<int> CountAsync()
    {
        return _rezervacijaRepository.CountAsync();
    }
}
