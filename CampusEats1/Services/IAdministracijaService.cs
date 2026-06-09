namespace CampusEats.Services;

public interface IAdministracijaService
{
    Task<AdministracijaStatistika> GetStatistikaAsync();
    Task<AdministracijaPregled> GetPregledAsync();
}

public class AdministracijaStatistika
{
    public int BrojKorisnika { get; set; }
    public int BrojObroka { get; set; }
    public int BrojRezervacija { get; set; }
    public int BrojAktivnihObroka { get; set; }
}

public class AdministracijaPregled
{
    public AdministracijaStatistika Statistika { get; set; } = new();
    public List<CampusEats.Models.AktivnostLog> Aktivnosti { get; set; } = [];
    public List<CampusEats.Models.Obavijest> Obavijesti { get; set; } = [];
    public List<CampusEats.Models.HistorijaIzmjene> HistorijaIzmjena { get; set; } = [];
}
