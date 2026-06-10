using CampusEats.Models;

namespace CampusEats.Services;

public interface IObavijestService
{
    Task KreirajAsync(int? korisnikId, int? rezervacijaId, string naslov, string poruka);
    Task KreirajZaUloguAsync(UlogaKorisnika uloga, int? rezervacijaId, string naslov, string poruka);
    Task<List<Obavijest>> GetForUserAsync(int korisnikId);
    Task<List<Obavijest>> GetLatestAsync(int count = 10);
}
