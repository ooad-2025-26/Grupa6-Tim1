using CampusEats.Models;
using CampusEats.Repositories;

namespace CampusEats.Services;

public class KorisnikService : IKorisnikService
{
    private readonly IKorisnikRepository _korisnikRepository;

    public KorisnikService(IKorisnikRepository korisnikRepository)
    {
        _korisnikRepository = korisnikRepository;
    }

    public Task<List<Korisnik>> GetAllAsync()
    {
        return _korisnikRepository.GetAllAsync();
    }

    public Task<List<Korisnik>> GetStudentsAsync()
    {
        return _korisnikRepository.GetStudentsAsync();
    }

    public Task<int> CountAsync()
    {
        return _korisnikRepository.CountAsync();
    }
}
