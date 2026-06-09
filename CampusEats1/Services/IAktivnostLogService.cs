using CampusEats.Models;

namespace CampusEats.Services;

public interface IAktivnostLogService
{
    Task ZabiljeziAsync(string akcija, string? entitet, int? entitetId, string opis);
    Task<List<AktivnostLog>> GetLatestAsync(int count = 10);
}
