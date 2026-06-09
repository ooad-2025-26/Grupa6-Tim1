namespace CampusEats.Services;

public interface IAdministracijaService
{
    Task<AdministracijaStatistika> GetStatistikaAsync();
}

public class AdministracijaStatistika
{
    public int BrojKorisnika { get; set; }
    public int BrojObroka { get; set; }
    public int BrojRezervacija { get; set; }
    public int BrojAktivnihObroka { get; set; }
}
