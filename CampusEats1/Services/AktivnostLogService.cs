using CampusEats.Data;
using CampusEats.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Services;

public class AktivnostLogService : IAktivnostLogService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AktivnostLogService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task ZabiljeziAsync(string akcija, string? entitet, int? entitetId, string opis)
    {
        _context.Aktivnosti.Add(new AktivnostLog
        {
            KorisnikEmail = _httpContextAccessor.HttpContext?.User.Identity?.Name,
            Akcija = akcija,
            Entitet = entitet,
            EntitetId = entitetId,
            Opis = opis,
            Vrijeme = DateTime.Now
        });

        await _context.SaveChangesAsync();
    }

    public Task<List<AktivnostLog>> GetLatestAsync(int count = 10)
    {
        return _context.Aktivnosti
            .OrderByDescending(aktivnost => aktivnost.Vrijeme)
            .Take(count)
            .ToListAsync();
    }
}
