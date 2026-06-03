using CampusEats.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampusEats.Interfaces
{
    public interface IMeniService
    {
        Task<List<Meni>> GetAllAsync();
        Task<Meni?> GetByIdAsync(int id);
        Task<Meni> CreateAsync(Meni meni);
        Task<bool> UpdateAsync(Meni meni);
        Task<bool> DeleteAsync(int id);
    }
}
