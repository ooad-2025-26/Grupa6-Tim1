using CampusEats.Interfaces;
using CampusEats.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampusEats.Services
{
    public class ObrokService : IObrokService
    {
        private readonly IObrokRepository _repo;

        public ObrokService(IObrokRepository repo)
        {
            _repo = repo;
        }

        public async Task<Obrok> CreateAsync(Obrok obrok)
        {
            await _repo.AddAsync(obrok);
            return obrok;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;
            await _repo.DeleteAsync(existing);
            return true;
        }

        public async Task<List<Obrok>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Obrok?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        // Category filtering using the Category property.
        public async Task<List<Obrok>> GetByCategoryAsync(string? category)
        {
            var all = await _repo.GetAllAsync();
            if (string.IsNullOrWhiteSpace(category) || category == "All") return all;
            var cat = category.Trim();
            return all.Where(o => string.Equals(o.Category ?? "Meals", cat, System.StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public async Task<bool> UpdateAsync(Obrok obrok)
        {
            if (!await _repo.ExistsAsync(obrok.Id)) return false;
            await _repo.UpdateAsync(obrok);
            return true;
        }
    }
}
