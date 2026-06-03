using CampusEats.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampusEats.Interfaces
{
    public interface IQRKodService
    {
        Task<List<QRKod>> GetAllAsync();
        Task<QRKod?> GetByIdAsync(int id);
        Task<QRKod> CreateAsync(QRKod qRKod);
        Task<bool> UpdateAsync(QRKod qRKod);
        Task<bool> DeleteAsync(int id);
    }
}
