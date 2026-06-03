using CampusEats.Data;
using CampusEats.Interfaces;
using CampusEats.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampusEats.Repositories
{
    public class QRKodRepository : IQRKodRepository
    {
        private readonly DataContext _context;

        public QRKodRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(QRKod qRKod)
        {
            await _context.QRKodovi.AddAsync(qRKod);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(QRKod qRKod)
        {
            _context.QRKodovi.Remove(qRKod);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.QRKodovi.AnyAsync(q => q.Id == id);
        }

        public async Task<List<QRKod>> GetAllAsync()
        {
            return await _context.QRKodovi.Include(q => q.Rezervacija).ToListAsync();
        }

        public async Task<QRKod?> GetByIdAsync(int id)
        {
            return await _context.QRKodovi.Include(q => q.Rezervacija).FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task UpdateAsync(QRKod qRKod)
        {
            _context.QRKodovi.Update(qRKod);
            await _context.SaveChangesAsync();
        }
    }
}
