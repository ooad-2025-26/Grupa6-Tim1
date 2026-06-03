using CampusEats.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampusEats.Interfaces
{
    public interface IObrokRepository
    {
        Task<List<Obrok>> GetAllAsync();
        Task<Obrok?> GetByIdAsync(int id);
        Task AddAsync(Obrok obrok);
        Task UpdateAsync(Obrok obrok);
        Task DeleteAsync(Obrok obrok);
        Task<bool> ExistsAsync(int id);
    }
}
