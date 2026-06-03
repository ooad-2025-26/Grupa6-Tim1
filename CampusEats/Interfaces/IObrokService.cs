using CampusEats.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampusEats.Interfaces
{
    public interface IObrokService
    {
        Task<List<Obrok>> GetAllAsync();
        Task<Obrok?> GetByIdAsync(int id);
        Task<List<Obrok>> GetByCategoryAsync(string? category);
        Task<Obrok> CreateAsync(Obrok obrok);
        Task<bool> UpdateAsync(Obrok obrok);
        Task<bool> DeleteAsync(int id);
    }
}
