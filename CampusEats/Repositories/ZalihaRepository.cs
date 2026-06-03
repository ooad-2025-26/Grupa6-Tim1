using CampusEats.Data;
using CampusEats.Interfaces;
using CampusEats.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampusEats.Repositories
{
    public class ZalihaRepository : IZalihaRepository
    {
        private readonly DataContext _context;

        public ZalihaRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Zaliha zaliha)
        {
            await _context.Zalihe.AddAsync(zaliha);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Zaliha zaliha)
        {
            _context.Zalihe.Remove(zaliha);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Zalihe.AnyAsync(z => z.Id == id);
        }

        public async Task<List<Zaliha>> GetAllAsync()
        {
            return await _context.Zalihe.ToListAsync();
        }

        public async Task<Zaliha?> GetByIdAsync(int id)
        {
            return await _context.Zalihe.FirstOrDefaultAsync(z => z.Id == id);
        }

        public async Task UpdateAsync(Zaliha zaliha)
        {
            _context.Zalihe.Update(zaliha);
            await _context.SaveChangesAsync();
        }
    }
}
