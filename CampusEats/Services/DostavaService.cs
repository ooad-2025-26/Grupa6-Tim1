using CampusEats.Interfaces;
using CampusEats.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampusEats.Services
{
    public class DostavaService : IDostavaService
    {
        private readonly IDostavaRepository _repo;

        public DostavaService(IDostavaRepository repo)
        {
            _repo = repo;
        }

        public async Task<Dostava> CreateAsync(Dostava dostava)
        {
            await _repo.AddAsync(dostava);
            return dostava;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;
            await _repo.DeleteAsync(existing);
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _repo.ExistsAsync(id);
        }

        public async Task<List<Dostava>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Dostava?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(Dostava dostava)
        {
            if (!await _repo.ExistsAsync(dostava.Id)) return false;
            await _repo.UpdateAsync(dostava);
            return true;
        }
    }
}
