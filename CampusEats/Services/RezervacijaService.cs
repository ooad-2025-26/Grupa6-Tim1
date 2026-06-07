using CampusEats.Interfaces;
using CampusEats.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampusEats.Services
{
    public class RezervacijaService : IRezervacijaService
    {
        private readonly IRezervacijaRepository _repo;

        public RezervacijaService(IRezervacijaRepository repo)
        {
            _repo = repo;
        }

        public async Task<Rezervacija> CreateReservationAsync(string userId, int obrokId)
        {
            var r = new Rezervacija
            {
                Datum = DateTime.Now,
                Status = StatusRezervacije.Kreirana,
                KorisnikId = userId,
                ObrokId = obrokId
            };
            await _repo.AddAsync(r);
            return r;
        }

        public async Task<bool> DeleteAsync(int id, string currentUserId, bool isAdmin)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;
            if (!isAdmin && existing.KorisnikId != currentUserId) return false;
            await _repo.DeleteAsync(existing);
            return true;
        }

        public async Task<List<Rezervacija>> GetAllAsync(string? currentUserId, bool canManageAll)
        {
            var all = await _repo.GetAllAsync();
            if (canManageAll) return all;
            if (string.IsNullOrEmpty(currentUserId)) return new List<Rezervacija>();
            return await _repo.GetByUserIdAsync(currentUserId);
        }

        public async Task<Rezervacija?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<List<Rezervacija>> GetByUserIdAsync(string userId)
        {
            return await _repo.GetByUserIdAsync(userId);
        }

        public async Task<bool> UpdateAsync(Rezervacija rezervacija, string currentUserId, bool canManageAll)
        {
            var existing = await _repo.GetByIdAsync(rezervacija.Id);
            if (existing == null) return false;
            if (!canManageAll && existing.KorisnikId != currentUserId) return false;
            // preserve owner
            rezervacija.KorisnikId = existing.KorisnikId;
            await _repo.UpdateAsync(rezervacija);
            return true;
        }

        public async Task UpdateStatusAsync(int id, StatusRezervacije status)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r == null) return;
            r.Status = status;
            await _repo.UpdateAsync(r);
        }
    }
}
