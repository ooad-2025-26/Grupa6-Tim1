using CampusEats.Models;

namespace CampusEats.Services;

public interface IKorisnikService
{
    Task<List<Korisnik>> GetAllAsync();
    Task<List<Korisnik>> GetStudentsAsync();
    Task<int> CountAsync();
}
