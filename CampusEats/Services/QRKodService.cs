using CampusEats.Interfaces;
using CampusEats.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampusEats.Services
{
    public class QRKodService : IQRKodService
    {
        private readonly IQRKodRepository _repo;

        public QRKodService(IQRKodRepository repo)
        {
            _repo = repo;
        }

        public async Task<QRKod> CreateAsync(QRKod qRKod)
        {
            await _repo.AddAsync(qRKod);
            return qRKod;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;
            await _repo.DeleteAsync(existing);
            return true;
        }

        public async Task<List<QRKod>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<QRKod?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(QRKod qRKod)
        {
            if (!await _repo.ExistsAsync(qRKod.Id)) return false;
            await _repo.UpdateAsync(qRKod);
            return true;
        }
    }
}
