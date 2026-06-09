using CampusEats.Data;
using CampusEats.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Repositories;

public class KorisnikRepository : IKorisnikRepository
{
    private readonly ApplicationDbContext _context;

    public KorisnikRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<Korisnik>> GetAllAsync()
    {
        return _context.Korisnici
            .OrderBy(korisnik => korisnik.Uloga)
            .ThenBy(korisnik => korisnik.Prezime)
            .ToListAsync();
    }

    public Task<List<Korisnik>> GetStudentsAsync()
    {
        return _context.Korisnici
            .Where(korisnik => korisnik.Uloga == UlogaKorisnika.Student)
            .OrderBy(korisnik => korisnik.Email)
            .ToListAsync();
    }

    public Task<int> CountAsync()
    {
        return _context.Korisnici.CountAsync();
    }
}
