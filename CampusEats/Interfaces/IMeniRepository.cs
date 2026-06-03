using CampusEats.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampusEats.Interfaces
{
    public interface IMeniRepository
    {
        Task<List<Meni>> GetAllAsync();
        Task<Meni?> GetByIdAsync(int id);
        Task AddAsync(Meni meni);
        Task UpdateAsync(Meni meni);
        Task DeleteAsync(Meni meni);
        Task<bool> ExistsAsync(int id);
    }
}
