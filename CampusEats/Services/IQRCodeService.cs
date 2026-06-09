using CampusEats.Models;

namespace CampusEats.Services;

public interface IQRCodeService
{
    Task<(bool Success, string Message, Rezervacija? Rezervacija)> EvidentirajAsync(
        string kod,
        bool jeStudent,
        bool jeKurir,
        bool jeRadnikMenze,
        bool jeAdministrator,
        int? studentId);
}
