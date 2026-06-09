using CampusEats.Models;

namespace CampusEats.Repositories;

public interface IRezervacijaRepository
{
    Task<List<Rezervacija>> GetAllAsync();
    Task<List<Rezervacija>> GetByKorisnikIdAsync(int korisnikId);
    Task<List<Rezervacija>> GetDeliveriesAsync();
    Task<Rezervacija?> GetByIdAsync(int id);
    Task<Rezervacija?> GetByIdWithDetailsAsync(int id);
    Task<Rezervacija?> GetByIdWithQrAsync(int id);
    Task AddAsync(Rezervacija rezervacija);
    void Update(Rezervacija rezervacija);
    void Remove(Rezervacija rezervacija);
    Task<bool> ExistsAsync(int id);
    Task<int> CountAsync();
    Task SaveChangesAsync();
}
