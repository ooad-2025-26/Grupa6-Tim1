using CampusEats.Data;
using CampusEats.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Services;

public class ObavijestService : IObavijestService
{
    private readonly ApplicationDbContext _context;

    public ObavijestService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task KreirajAsync(int? korisnikId, int? rezervacijaId, string naslov, string poruka)
    {
        _context.Obavijesti.Add(new Obavijest
        {
            KorisnikId = korisnikId,
            RezervacijaId = rezervacijaId,
            Naslov = naslov,
            Poruka = poruka,
            DatumSlanja = DateTime.Now
        });

        await _context.SaveChangesAsync();
    }

    public async Task KreirajZaUloguAsync(UlogaKorisnika uloga, int? rezervacijaId, string naslov, string poruka)
    {
        var korisnikIds = await _context.Korisnici
            .Where(korisnik => korisnik.Uloga == uloga)
            .Select(korisnik => korisnik.Id)
            .ToListAsync();

        foreach (var korisnikId in korisnikIds)
        {
            _context.Obavijesti.Add(new Obavijest
            {
                KorisnikId = korisnikId,
                RezervacijaId = rezervacijaId,
                Naslov = naslov,
                Poruka = poruka,
                DatumSlanja = DateTime.Now
            });
        }

        await _context.SaveChangesAsync();
    }

    public Task<List<Obavijest>> GetForUserAsync(int korisnikId)
    {
        return _context.Obavijesti
            .Include(obavijest => obavijest.Rezervacija)
            .ThenInclude(rezervacija => rezervacija!.Obrok)
            .Where(obavijest => obavijest.KorisnikId == korisnikId)
            .OrderByDescending(obavijest => obavijest.DatumSlanja)
            .ToListAsync();
    }

    public Task<List<Obavijest>> GetLatestAsync(int count = 10)
    {
        return _context.Obavijesti
            .Include(obavijest => obavijest.Korisnik)
            .OrderByDescending(obavijest => obavijest.DatumSlanja)
            .Take(count)
            .ToListAsync();
    }
}
