namespace CampusEats.Services;

public class AdministracijaService : IAdministracijaService
{
    private readonly IAktivnostLogService _aktivnostLogService;
    private readonly IHistorijaIzmjeneService _historijaIzmjeneService;
    private readonly IKorisnikService _korisnikService;
    private readonly IObrokService _obrokService;
    private readonly IObavijestService _obavijestService;
    private readonly IRezervacijaService _rezervacijaService;

    public AdministracijaService(
        IKorisnikService korisnikService,
        IObrokService obrokService,
        IRezervacijaService rezervacijaService,
        IObavijestService obavijestService,
        IAktivnostLogService aktivnostLogService,
        IHistorijaIzmjeneService historijaIzmjeneService)
    {
        _korisnikService = korisnikService;
        _obrokService = obrokService;
        _rezervacijaService = rezervacijaService;
        _obavijestService = obavijestService;
        _aktivnostLogService = aktivnostLogService;
        _historijaIzmjeneService = historijaIzmjeneService;
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

    public async Task<AdministracijaPregled> GetPregledAsync()
    {
        return new AdministracijaPregled
        {
            Statistika = await GetStatistikaAsync(),
            Aktivnosti = await _aktivnostLogService.GetLatestAsync(10),
            Obavijesti = await _obavijestService.GetLatestAsync(10),
            HistorijaIzmjena = await _historijaIzmjeneService.GetLatestAsync(10)
        };
    }
}
