using CampusEats.Data;
using CampusEats.Interfaces;
using CampusEats.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampusEats.Repositories
{
    public class ObrokRepository : IObrokRepository
    {
        private readonly DataContext _context;

        public ObrokRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Obrok obrok)
        {
            await _context.Obroci.AddAsync(obrok);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Obrok obrok)
        {
            _context.Obroci.Remove(obrok);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Obroci.AnyAsync(o => o.Id == id);
        }

        public async Task<List<Obrok>> GetAllAsync()
        {
            return await _context.Obroci.ToListAsync();
        }

        public async Task<Obrok?> GetByIdAsync(int id)
        {
            return await _context.Obroci.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task UpdateAsync(Obrok obrok)
        {
            _context.Obroci.Update(obrok);
            await _context.SaveChangesAsync();
        }
    }
}
