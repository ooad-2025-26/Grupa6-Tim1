using CampusEats.Models;

namespace CampusEats.Services;

public interface IHistorijaIzmjeneService
{
    Task ZabiljeziAsync(string entitet, int entitetId, string tipIzmjene, string opis);
    Task<List<HistorijaIzmjene>> GetLatestAsync(int count = 10);
}
