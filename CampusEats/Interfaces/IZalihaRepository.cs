using CampusEats.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampusEats.Interfaces
{
    public interface IZalihaRepository
    {
        Task<List<Zaliha>> GetAllAsync();
        Task<Zaliha?> GetByIdAsync(int id);
        Task AddAsync(Zaliha zaliha);
        Task UpdateAsync(Zaliha zaliha);
        Task DeleteAsync(Zaliha zaliha);
        Task<bool> ExistsAsync(int id);
    }
}
