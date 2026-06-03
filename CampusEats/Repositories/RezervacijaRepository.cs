using CampusEats.Data;
using CampusEats.Interfaces;
using CampusEats.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampusEats.Repositories
{
    public class RezervacijaRepository : IRezervacijaRepository
    {
        private readonly DataContext _context;

        public RezervacijaRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Rezervacija rezervacija)
        {
            await _context.Rezervacije.AddAsync(rezervacija);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Rezervacija rezervacija)
        {
            _context.Rezervacije.Remove(rezervacija);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Rezervacije.AnyAsync(r => r.Id == id);
        }

        public async Task<List<Rezervacija>> GetAllAsync()
        {
            return await _context.Rezervacije.Include(r => r.Korisnik).Include(r => r.Obrok).ToListAsync();
        }

        public async Task<Rezervacija?> GetByIdAsync(int id)
        {
            return await _context.Rezervacije.Include(r => r.Korisnik).Include(r => r.Obrok).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Rezervacija>> GetByUserIdAsync(string userId)
        {
            return await _context.Rezervacije.Include(r => r.Korisnik).Include(r => r.Obrok).Where(r => r.KorisnikId == userId).ToListAsync();
        }

        public async Task UpdateAsync(Rezervacija rezervacija)
        {
            _context.Rezervacije.Update(rezervacija);
            await _context.SaveChangesAsync();
        }
    }
}
