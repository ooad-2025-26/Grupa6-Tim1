using CampusEats.Models;

namespace CampusEats.Services;

public interface IRezervacijaService
{
    Task<List<Rezervacija>> GetAllAsync();
    Task<List<Rezervacija>> GetDeliveriesAsync();
    Task<Rezervacija?> GetByIdAsync(int? id);
    Task<Rezervacija?> GetByIdWithDetailsAsync(int? id);
    Task<(bool Success, string? Error)> CreateAsync(Rezervacija rezervacija);
    Task<bool> UpdateAsync(int id, Rezervacija rezervacija);
    Task<bool> UpdateStatusAsync(int id, StatusRezervacije status);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<int> CountAsync();
}
