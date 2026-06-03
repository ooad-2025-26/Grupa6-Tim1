using CampusEats.Interfaces;
using CampusEats.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampusEats.Services
{
    public class MeniService : IMeniService
    {
        private readonly IMeniRepository _repo;

        public MeniService(IMeniRepository repo)
        {
            _repo = repo;
        }

        public async Task<Meni> CreateAsync(Meni meni)
        {
            await _repo.AddAsync(meni);
            return meni;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;
            await _repo.DeleteAsync(existing);
            return true;
        }

        public async Task<List<Meni>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Meni?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(Meni meni)
        {
            if (!await _repo.ExistsAsync(meni.Id)) return false;
            await _repo.UpdateAsync(meni);
            return true;
        }
    }
}
