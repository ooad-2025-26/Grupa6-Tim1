using CampusEats.Models;
using CampusEats.Repositories;

namespace CampusEats.Services;

public class QRCodeService : IQRCodeService
{
    private const string KurirskiKodPrefix = "KURIR:";
    private readonly IQRCodeRepository _qrCodeRepository;

    public QRCodeService(IQRCodeRepository qrCodeRepository)
    {
        _qrCodeRepository = qrCodeRepository;
    }

    public async Task<(bool Success, string Message, Rezervacija? Rezervacija)> EvidentirajAsync(
        string kod,
        bool jeStudent,
        bool jeKurir,
        bool jeRadnikMenze,
        bool jeAdministrator,
        int? studentId)
    {
        if (string.IsNullOrWhiteSpace(kod))
        {
            return (false, "Unesite ili skenirajte QR kod.", null);
        }

        kod = kod.Trim();

        if (kod.StartsWith(KurirskiKodPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return await EvidentirajKurirskiKodAsync(kod, jeStudent, jeAdministrator, studentId);
        }

        var qrKod = await _qrCodeRepository.GetValidByCodeAsync(kod);
        if (qrKod?.Rezervacija is null)
        {
            return (false, "QR kod nije pronadjen ili vise nije validan.", null);
        }

        if (jeStudent && !jeAdministrator)
        {
            return (false, "Student za dostavu skenira kurirski QR kod, ne QR kod narudzbe.", qrKod.Rezervacija);
        }

        if (qrKod.Rezervacija.NacinPreuzimanja == NacinPreuzimanja.Dostava)
        {
            if (!jeKurir && !jeAdministrator)
            {
                return (false, "Narudzbu za dostavu moze preuzeti samo kurir.", qrKod.Rezervacija);
            }

            qrKod.Rezervacija.Status = StatusRezervacije.Preuzeta;
            qrKod.Validan = false;
            await _qrCodeRepository.SaveChangesAsync();

            return (true, $"Narudzba #{qrKod.Rezervacija.Id} je evidentirana kao Preuzeta. Student sada skenira kurirski QR kod za potvrdu dostave.", qrKod.Rezervacija);
        }

        if (!jeRadnikMenze && !jeAdministrator)
        {
            return (false, "Licno preuzimanje moze evidentirati samo radnik menze.", qrKod.Rezervacija);
        }

        qrKod.Rezervacija.Status = StatusRezervacije.Preuzeta;
        qrKod.Validan = false;

        await _qrCodeRepository.SaveChangesAsync();

        return (true, $"Narudzba #{qrKod.Rezervacija.Id} je evidentirana kao Preuzeta.", qrKod.Rezervacija);
    }

    private async Task<(bool Success, string Message, Rezervacija? Rezervacija)> EvidentirajKurirskiKodAsync(
        string kod,
        bool jeStudent,
        bool jeAdministrator,
        int? studentId)
    {
        if (!jeStudent && !jeAdministrator)
        {
            return (false, "Kurirski QR kod skenira student nakon sto kurir donese narudzbu.", null);
        }

        var dijelovi = kod.Split(':', 3, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (dijelovi.Length != 3 || !int.TryParse(dijelovi[1], out var rezervacijaId))
        {
            return (false, "Kurirski QR kod nije ispravan.", null);
        }

        var qrKod = await _qrCodeRepository.GetByCodeAsync(dijelovi[2]);
        if (qrKod?.Rezervacija is null || qrKod.Rezervacija.Id != rezervacijaId)
        {
            return (false, "Kurirski QR kod nije povezan sa validnom narudzbom.", null);
        }

        if (qrKod.Rezervacija.NacinPreuzimanja != NacinPreuzimanja.Dostava)
        {
            return (false, "Kurirski QR kod vrijedi samo za dostavu.", qrKod.Rezervacija);
        }

        if (jeStudent && !jeAdministrator && qrKod.Rezervacija.KorisnikId != studentId)
        {
            return (false, "Ovo nije vasa narudzba.", null);
        }

        if (qrKod.Rezervacija.Status != StatusRezervacije.Preuzeta)
        {
            return (false, "Narudzba prvo mora biti evidentirana kao Preuzeta od strane kurira.", qrKod.Rezervacija);
        }

        qrKod.Rezervacija.Status = StatusRezervacije.Dostavljena;
        await _qrCodeRepository.SaveChangesAsync();

        return (true, $"Narudzba #{qrKod.Rezervacija.Id} je evidentirana kao Dostavljena.", qrKod.Rezervacija);
    }
}
