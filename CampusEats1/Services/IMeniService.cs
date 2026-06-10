using CampusEats.Models;

namespace CampusEats.Services;

public interface IMeniService
{
    Task<List<Meni>> GetAllAsync();
    Task<List<Meni>> GetVisibleAsync();
    Task<List<Obrok>> GetAvailableMenuObrociAsync();
    Task<Meni?> GetByIdAsync(int? id);
    Task<Meni?> GetByIdWithObrokAsync(int? id);
    Task CreateAsync(Meni meni);
    Task<bool> UpdateAsync(int id, Meni meni);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
