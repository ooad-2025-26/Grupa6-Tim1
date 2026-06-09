using CampusEats.Models;
using CampusEats.Repositories;

namespace CampusEats.Services;

public class QRCodeService : IQRCodeService
{
    private readonly IQRCodeRepository _qrCodeRepository;

    public QRCodeService(IQRCodeRepository qrCodeRepository)
    {
        _qrCodeRepository = qrCodeRepository;
    }

    public async Task<(bool Success, string Message, Rezervacija? Rezervacija)> EvidentirajAsync(string kod)
    {
        if (string.IsNullOrWhiteSpace(kod))
        {
            return (false, "Unesite ili skenirajte QR kod.", null);
        }

        var qrKod = await _qrCodeRepository.GetValidByCodeAsync(kod);
        if (qrKod?.Rezervacija is null)
        {
            return (false, "QR kod nije pronađen ili više nije validan.", null);
        }

        qrKod.Rezervacija.Status = qrKod.Rezervacija.NacinPreuzimanja == NacinPreuzimanja.Dostava
            ? StatusRezervacije.Dostavljena
            : StatusRezervacije.Preuzeta;
        qrKod.Validan = false;

        await _qrCodeRepository.SaveChangesAsync();

        var message = $"Narudžba #{qrKod.Rezervacija.Id} je evidentirana kao {qrKod.Rezervacija.Status}.";
        return (true, message, qrKod.Rezervacija);
    }
}
