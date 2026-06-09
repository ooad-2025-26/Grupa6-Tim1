using CampusEats.Models;

namespace CampusEats.Repositories;

public interface IQRCodeRepository
{
    Task<QRKod?> GetValidByCodeAsync(string kod);
    Task SaveChangesAsync();
}
