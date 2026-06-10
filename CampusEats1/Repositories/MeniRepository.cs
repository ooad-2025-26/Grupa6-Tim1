using CampusEats.Data;
using CampusEats.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Repositories;

public class MeniRepository : IMeniRepository
{
    private readonly ApplicationDbContext _context;

    public MeniRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<Meni>> GetAllAsync()
    {
        return _context.Meniji
            .Include(meni => meni.Obrok)
            .OrderBy(meni => meni.Datum)
            .ToListAsync();
    }

    public Task<List<Meni>> GetVisibleAsync()
    {
        return _context.Meniji
            .Include(meni => meni.Obrok)
            .Where(meni => meni.Obrok != null && meni.Obrok.Dostupan && meni.Obrok.Kolicina > 0)
            .OrderBy(meni => meni.Datum)
            .ThenBy(meni => meni.Obrok!.Naziv)
            .ToListAsync();
    }

    public Task<List<Obrok>> GetAvailableMenuObrociAsync()
    {
        return _context.Meniji
            .Include(meni => meni.Obrok)
            .Where(meni => meni.Obrok != null && meni.Obrok.Dostupan && meni.Obrok.Kolicina > 0)
            .Select(meni => meni.Obrok!)
            .Distinct()
            .OrderBy(obrok => obrok.Naziv)
            .ToListAsync();
    }

    public Task<Meni?> GetByIdAsync(int id)
    {
        return _context.Meniji.FirstOrDefaultAsync(meni => meni.Id == id);
    }

    public Task<Meni?> GetByIdWithObrokAsync(int id)
    {
        return _context.Meniji
            .Include(meni => meni.Obrok)
            .FirstOrDefaultAsync(meni => meni.Id == id);
    }

    public async Task AddAsync(Meni meni)
    {
        await _context.Meniji.AddAsync(meni);
    }

    public void Update(Meni meni)
    {
        _context.Meniji.Update(meni);
    }

    public void Remove(Meni meni)
    {
        _context.Meniji.Remove(meni);
    }

    public Task<bool> ExistsAsync(int id)
    {
        return _context.Meniji.AnyAsync(meni => meni.Id == id);
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
