using CampusEats.Data;
using CampusEats.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Repositories;

public class QRCodeRepository : IQRCodeRepository
{
    private readonly ApplicationDbContext _context;

    public QRCodeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<QRKod?> GetValidByCodeAsync(string kod)
    {
        return _context.QRKodovi
            .Include(qrKod => qrKod.Rezervacija)
            .ThenInclude(rezervacija => rezervacija!.Obrok)
            .Include(qrKod => qrKod.Rezervacija)
            .ThenInclude(rezervacija => rezervacija!.Korisnik)
            .Include(qrKod => qrKod.Rezervacija)
            .ThenInclude(rezervacija => rezervacija!.Kurir)
            .FirstOrDefaultAsync(qrKod => qrKod.Kod == kod.Trim() && qrKod.Validan);
    }

    public Task<QRKod?> GetByCodeAsync(string kod)
    {
        return _context.QRKodovi
            .Include(qrKod => qrKod.Rezervacija)
            .ThenInclude(rezervacija => rezervacija!.Obrok)
            .Include(qrKod => qrKod.Rezervacija)
            .ThenInclude(rezervacija => rezervacija!.Korisnik)
            .Include(qrKod => qrKod.Rezervacija)
            .ThenInclude(rezervacija => rezervacija!.Kurir)
            .FirstOrDefaultAsync(qrKod => qrKod.Kod == kod.Trim());
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
