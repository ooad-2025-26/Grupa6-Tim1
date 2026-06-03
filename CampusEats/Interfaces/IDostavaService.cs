using CampusEats.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampusEats.Interfaces
{
    public interface IDostavaService
    {
        Task<List<Dostava>> GetAllAsync();
        Task<Dostava?> GetByIdAsync(int id);
        Task<Dostava> CreateAsync(Dostava dostava);
        Task<bool> UpdateAsync(Dostava dostava);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
