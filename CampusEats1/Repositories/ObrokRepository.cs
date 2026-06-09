using CampusEats.Data;
using CampusEats.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Repositories;

public class ObrokRepository : IObrokRepository
{
    private readonly ApplicationDbContext _context;

    public ObrokRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<Obrok>> GetAllAsync()
    {
        return _context.Obroci
            .OrderByDescending(obrok => obrok.Dostupan)
            .ThenBy(obrok => obrok.Naziv)
            .ToListAsync();
    }

    public Task<List<Obrok>> GetAvailableAsync()
    {
        return _context.Obroci
            .Where(obrok => obrok.Dostupan)
            .OrderBy(obrok => obrok.Naziv)
            .ToListAsync();
    }

    public Task<Obrok?> GetByIdAsync(int id)
    {
        return _context.Obroci.FirstOrDefaultAsync(obrok => obrok.Id == id);
    }

    public async Task AddAsync(Obrok obrok)
    {
        await _context.Obroci.AddAsync(obrok);
    }

    public void Update(Obrok obrok)
    {
        _context.Obroci.Update(obrok);
    }

    public void Remove(Obrok obrok)
    {
        _context.Obroci.Remove(obrok);
    }

    public Task<bool> ExistsAsync(int id)
    {
        return _context.Obroci.AnyAsync(obrok => obrok.Id == id);
    }

    public Task<int> CountAsync()
    {
        return _context.Obroci.CountAsync();
    }

    public Task<int> CountAvailableAsync()
    {
        return _context.Obroci.CountAsync(obrok => obrok.Dostupan);
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
