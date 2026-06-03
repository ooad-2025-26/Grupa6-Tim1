using CampusEats.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampusEats.Interfaces
{
    public interface IRezervacijaService
    {
        Task<List<Rezervacija>> GetAllAsync(string? currentUserId, bool isAdmin);
        Task<Rezervacija?> GetByIdAsync(int id);
        Task<List<Rezervacija>> GetByUserIdAsync(string userId);
        Task<Rezervacija> CreateReservationAsync(string userId, int obrokId);
        Task<bool> UpdateAsync(Rezervacija rezervacija, string currentUserId, bool isAdmin);
        Task<bool> DeleteAsync(int id, string currentUserId, bool isAdmin);
    }
}
