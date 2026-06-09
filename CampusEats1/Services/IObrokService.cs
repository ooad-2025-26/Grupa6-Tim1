using CampusEats.Models;

namespace CampusEats.Services;

public interface IObrokService
{
    Task<List<Obrok>> GetAllAsync();
    Task<List<Obrok>> GetAvailableAsync();
    Task<Obrok?> GetByIdAsync(int? id);
    Task CreateAsync(Obrok obrok);
    Task<bool> UpdateAsync(int id, Obrok obrok);
    Task<Obrok?> DeletePreviewAsync(int? id);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<int> CountAsync();
    Task<int> CountAvailableAsync();
}
