using CampusEats.Models;

namespace CampusEats.Repositories;

public interface IKorisnikRepository
{
    Task<List<Korisnik>> GetAllAsync();
    Task<List<Korisnik>> GetStudentsAsync();
    Task<int> CountAsync();
}
