using CampusEats.Models;
using CampusEats.Repositories;

namespace CampusEats.Services;

public class RezervacijaService : IRezervacijaService
{
    private readonly IObrokRepository _obrokRepository;
    private readonly IRezervacijaRepository _rezervacijaRepository;

    public RezervacijaService(IRezervacijaRepository rezervacijaRepository, IObrokRepository obrokRepository)
    {
        _rezervacijaRepository = rezervacijaRepository;
        _obrokRepository = obrokRepository;
    }

    public Task<List<Rezervacija>> GetAllAsync()
    {
        return _rezervacijaRepository.GetAllAsync();
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
