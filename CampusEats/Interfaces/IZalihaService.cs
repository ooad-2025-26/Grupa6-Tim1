using CampusEats.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampusEats.Interfaces
{
    public interface IZalihaService
    {
        Task<List<Zaliha>> GetAllAsync();
        Task<Zaliha?> GetByIdAsync(int id);
        Task<Zaliha> CreateAsync(Zaliha zaliha);
        Task<bool> UpdateAsync(Zaliha zaliha);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
