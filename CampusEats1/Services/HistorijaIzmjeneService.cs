using CampusEats.Data;
using CampusEats.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Services;

public class HistorijaIzmjeneService : IHistorijaIzmjeneService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HistorijaIzmjeneService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task ZabiljeziAsync(string entitet, int entitetId, string tipIzmjene, string opis)
    {
        _context.HistorijaIzmjena.Add(new HistorijaIzmjene
        {
            Entitet = entitet,
            EntitetId = entitetId,
            TipIzmjene = tipIzmjene,
            KorisnikEmail = _httpContextAccessor.HttpContext?.User.Identity?.Name,
            Opis = opis,
            Vrijeme = DateTime.Now
        });

        await _context.SaveChangesAsync();
    }

    public Task<List<HistorijaIzmjene>> GetLatestAsync(int count = 10)
    {
        return _context.HistorijaIzmjena
            .OrderByDescending(izmjena => izmjena.Vrijeme)
            .Take(count)
            .ToListAsync();
    }
}
