using CampusEats.Models;

namespace CampusEats.Services;

public interface IPreporukaService
{
    Task<List<Obrok>> GetPreporukeAsync(int korisnikId, int count = 5);
}
