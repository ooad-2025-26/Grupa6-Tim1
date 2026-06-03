using CampusEats.Data;
using CampusEats.Interfaces;
using CampusEats.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampusEats.Repositories
{
    public class DostavaRepository : IDostavaRepository
    {
        private readonly DataContext _context;

        public DostavaRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Dostava dostava)
        {
            await _context.Dostave.AddAsync(dostava);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Dostava dostava)
        {
            _context.Dostave.Remove(dostava);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Dostave.AnyAsync(d => d.Id == id);
        }

        public async Task<List<Dostava>> GetAllAsync()
        {
            return await _context.Dostave.Include(d => d.Rezervacija).ToListAsync();
        }

        public async Task<Dostava?> GetByIdAsync(int id)
        {
            return await _context.Dostave.Include(d => d.Rezervacija).FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task UpdateAsync(Dostava dostava)
        {
            _context.Dostave.Update(dostava);
            await _context.SaveChangesAsync();
        }
    }
}
