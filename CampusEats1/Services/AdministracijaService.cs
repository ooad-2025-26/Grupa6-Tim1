namespace CampusEats.Services;

public class AdministracijaService : IAdministracijaService
{
    private readonly IKorisnikService _korisnikService;
    private readonly IObrokService _obrokService;
    private readonly IRezervacijaService _rezervacijaService;

    public AdministracijaService(
        IKorisnikService korisnikService,
        IObrokService obrokService,
        IRezervacijaService rezervacijaService)
    {
        _korisnikService = korisnikService;
        _obrokService = obrokService;
        _rezervacijaService = rezervacijaService;
    }

    public async Task<AdministracijaStatistika> GetStatistikaAsync()
    {
        return new AdministracijaStatistika
        {
            BrojKorisnika = await _korisnikService.CountAsync(),
            BrojObroka = await _obrokService.CountAsync(),
            BrojRezervacija = await _rezervacijaService.CountAsync(),
            BrojAktivnihObroka = await _obrokService.CountAvailableAsync()
        };
    }
}
