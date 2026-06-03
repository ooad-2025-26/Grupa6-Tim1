using CampusEats.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampusEats.Interfaces
{
    public interface IDostavaRepository
    {
        Task<List<Dostava>> GetAllAsync();
        Task<Dostava?> GetByIdAsync(int id);
        Task AddAsync(Dostava dostava);
        Task UpdateAsync(Dostava dostava);
        Task DeleteAsync(Dostava dostava);
        Task<bool> ExistsAsync(int id);
    }
}
