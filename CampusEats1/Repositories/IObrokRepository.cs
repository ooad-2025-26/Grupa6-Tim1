using CampusEats.Models;

namespace CampusEats.Repositories;

public interface IObrokRepository
{
    Task<List<Obrok>> GetAllAsync();
    Task<List<Obrok>> GetAvailableAsync();
    Task<Obrok?> GetByIdAsync(int id);
    Task AddAsync(Obrok obrok);
    void Update(Obrok obrok);
    void Remove(Obrok obrok);
    Task<bool> ExistsAsync(int id);
    Task<int> CountAsync();
    Task<int> CountAvailableAsync();
    Task SaveChangesAsync();
}
