using CampusEats.Interfaces;
using CampusEats.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampusEats.Services
{
    public class ZalihaService : IZalihaService
    {
        private readonly IZalihaRepository _repo;

        public ZalihaService(IZalihaRepository repo)
        {
            _repo = repo;
        }

        public async Task<Zaliha> CreateAsync(Zaliha zaliha)
        {
            await _repo.AddAsync(zaliha);
            return zaliha;
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

        public async Task<List<Zaliha>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Zaliha?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(Zaliha zaliha)
        {
            if (!await _repo.ExistsAsync(zaliha.Id)) return false;
            await _repo.UpdateAsync(zaliha);
            return true;
        }
    }
}
