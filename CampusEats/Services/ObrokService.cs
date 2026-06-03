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

        // Simple category filtering by checking if Naziv or Opis contains category text.
        public async Task<List<Obrok>> GetByCategoryAsync(string? category)
        {
            var all = await _repo.GetAllAsync();
            if (string.IsNullOrWhiteSpace(category)) return all;
            var cat = category.Trim().ToLowerInvariant();
            return all.Where(o => (o.Naziv ?? string.Empty).ToLowerInvariant().Contains(cat)
                              || (o.Opis ?? string.Empty).ToLowerInvariant().Contains(cat)).ToList();
        }

        public async Task<bool> UpdateAsync(Obrok obrok)
        {
            if (!await _repo.ExistsAsync(obrok.Id)) return false;
            await _repo.UpdateAsync(obrok);
            return true;
        }
    }
}
