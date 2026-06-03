using CampusEats.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampusEats.Interfaces
{
    public interface IRezervacijaRepository
    {
        Task<List<Rezervacija>> GetAllAsync();
        Task<Rezervacija?> GetByIdAsync(int id);
        Task<List<Rezervacija>> GetByUserIdAsync(string userId);
        Task AddAsync(Rezervacija rezervacija);
        Task UpdateAsync(Rezervacija rezervacija);
        Task DeleteAsync(Rezervacija rezervacija);
        Task<bool> ExistsAsync(int id);
    }
}
