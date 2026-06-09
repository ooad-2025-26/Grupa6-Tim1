using CampusEats.Data;
using CampusEats.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Services;

public class PreporukaService : IPreporukaService
{
    private static readonly string[] MesniPojmovi = ["piletina", "meso", "govedina", "teletina", "riba", "tuna", "kobasica"];
    private readonly ApplicationDbContext _context;

    public PreporukaService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Obrok>> GetPreporukeAsync(int korisnikId, int count = 5)
    {
        var korisnik = await _context.Korisnici.FindAsync(korisnikId);
        if (korisnik is null)
        {
            return [];
        }

        var obroci = await _context.Obroci
            .Where(obrok => obrok.Dostupan && obrok.Kolicina > 0)
            .ToListAsync();

        var prethodniObrokIds = await _context.Rezervacije
            .Where(rezervacija => rezervacija.KorisnikId == korisnikId)
            .GroupBy(rezervacija => rezervacija.ObrokId)
            .Select(grupa => new { ObrokId = grupa.Key, Broj = grupa.Count() })
            .ToDictionaryAsync(item => item.ObrokId, item => item.Broj);

        var alergije = SplitTerms(korisnik.Alergije);
        var omiljeno = SplitTerms(korisnik.OmiljenaHrana);

        return obroci
            .Where(obrok => !ContainsAny($"{obrok.Naziv} {obrok.Sastojci}", alergije))
            .Where(obrok => !korisnik.Vegetarijanac || !ContainsAny($"{obrok.Naziv} {obrok.Sastojci}", MesniPojmovi))
            .Select(obrok => new
            {
                Obrok = obrok,
                Score = Score(obrok, prethodniObrokIds, omiljeno)
            })
            .OrderByDescending(item => item.Score)
            .ThenBy(item => item.Obrok.Naziv)
            .Take(count)
            .Select(item => item.Obrok)
            .ToList();
    }

    private static int Score(Obrok obrok, Dictionary<int, int> prethodniObrokIds, string[] omiljeno)
    {
        var score = prethodniObrokIds.GetValueOrDefault(obrok.Id) * 3;
        var tekst = $"{obrok.Naziv} {obrok.Sastojci}";
        score += omiljeno.Count(term => tekst.Contains(term, StringComparison.OrdinalIgnoreCase)) * 5;
        score += obrok.Kolicina > 0 ? 1 : 0;
        return score;
    }

    private static string[] SplitTerms(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? []
            : value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

    private static bool ContainsAny(string text, IEnumerable<string> terms)
    {
        return terms.Any(term => text.Contains(term, StringComparison.OrdinalIgnoreCase));
    }
}
