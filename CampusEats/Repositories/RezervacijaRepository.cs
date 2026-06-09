using CampusEats.Data;
using CampusEats.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Repositories;

public class RezervacijaRepository : IRezervacijaRepository
{
    private readonly ApplicationDbContext _context;

    public RezervacijaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<Rezervacija>> GetAllAsync()
    {
        return _context.Rezervacije
            .Include(rezervacija => rezervacija.Korisnik)
            .Include(rezervacija => rezervacija.Obrok)
            .Include(rezervacija => rezervacija.QRKod)
            .OrderByDescending(rezervacija => rezervacija.Datum)
            .ToListAsync();
    }

    public Task<List<Rezervacija>> GetByKorisnikIdAsync(int korisnikId)
    {
        return _context.Rezervacije
            .Include(rezervacija => rezervacija.Korisnik)
            .Include(rezervacija => rezervacija.Obrok)
            .Include(rezervacija => rezervacija.QRKod)
            .Where(rezervacija => rezervacija.KorisnikId == korisnikId)
            .OrderByDescending(rezervacija => rezervacija.Datum)
            .ToListAsync();
    }

    public Task<List<Rezervacija>> GetDeliveriesAsync()
    {
        return _context.Rezervacije
            .Include(rezervacija => rezervacija.Korisnik)
            .Include(rezervacija => rezervacija.Obrok)
            .Include(rezervacija => rezervacija.QRKod)
            .Where(rezervacija => rezervacija.NacinPreuzimanja == NacinPreuzimanja.Dostava)
            .OrderBy(rezervacija => rezervacija.TerminPreuzimanja)
            .ToListAsync();
    }

    public Task<Rezervacija?> GetByIdAsync(int id)
    {
        return _context.Rezervacije.FirstOrDefaultAsync(rezervacija => rezervacija.Id == id);
    }

    public Task<Rezervacija?> GetByIdWithDetailsAsync(int id)
    {
        return _context.Rezervacije
            .Include(rezervacija => rezervacija.Korisnik)
            .Include(rezervacija => rezervacija.Obrok)
            .Include(rezervacija => rezervacija.QRKod)
            .FirstOrDefaultAsync(rezervacija => rezervacija.Id == id);
    }

    public Task<Rezervacija?> GetByIdWithQrAsync(int id)
    {
        return _context.Rezervacije
            .Include(rezervacija => rezervacija.QRKod)
            .FirstOrDefaultAsync(rezervacija => rezervacija.Id == id);
    }

    public async Task AddAsync(Rezervacija rezervacija)
    {
        await _context.Rezervacije.AddAsync(rezervacija);
    }

    public void Update(Rezervacija rezervacija)
    {
        _context.Rezervacije.Update(rezervacija);
    }

    public void Remove(Rezervacija rezervacija)
    {
        _context.Rezervacije.Remove(rezervacija);
    }

    public Task<bool> ExistsAsync(int id)
    {
        return _context.Rezervacije.AnyAsync(rezervacija => rezervacija.Id == id);
    }

    public Task<int> CountAsync()
    {
        return _context.Rezervacije.CountAsync();
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
