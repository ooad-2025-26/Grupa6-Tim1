using CampusEats.Data;
using CampusEats.Interfaces;
using CampusEats.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampusEats.Repositories
{
    public class MeniRepository : IMeniRepository
    {
        private readonly DataContext _context;

        public MeniRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Meni meni)
        {
            await _context.Meniji.AddAsync(meni);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Meni meni)
        {
            _context.Meniji.Remove(meni);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Meniji.AnyAsync(m => m.Id == id);
        }

        public async Task<List<Meni>> GetAllAsync()
        {
            return await _context.Meniji.ToListAsync();
        }

        public async Task<Meni?> GetByIdAsync(int id)
        {
            return await _context.Meniji.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task UpdateAsync(Meni meni)
        {
            _context.Meniji.Update(meni);
            await _context.SaveChangesAsync();
        }
    }
}
