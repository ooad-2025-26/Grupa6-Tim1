using CampusEats.Models;

namespace CampusEats.Repositories;

public interface IMeniRepository
{
    Task<List<Meni>> GetAllAsync();
    Task<List<Meni>> GetVisibleAsync();
    Task<List<Obrok>> GetAvailableMenuObrociAsync();
    Task<Meni?> GetByIdAsync(int id);
    Task<Meni?> GetByIdWithObrokAsync(int id);
    Task AddAsync(Meni meni);
    void Update(Meni meni);
    void Remove(Meni meni);
    Task<bool> ExistsAsync(int id);
    Task SaveChangesAsync();
}
