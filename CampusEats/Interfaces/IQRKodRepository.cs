using CampusEats.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampusEats.Interfaces
{
    public interface IQRKodRepository
    {
        Task<List<QRKod>> GetAllAsync();
        Task<QRKod?> GetByIdAsync(int id);
        Task AddAsync(QRKod qRKod);
        Task UpdateAsync(QRKod qRKod);
        Task DeleteAsync(QRKod qRKod);
        Task<bool> ExistsAsync(int id);
    }
}
