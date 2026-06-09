using CampusEats.Models;
using CampusEats.Repositories;

namespace CampusEats.Services;

public class ObrokService : IObrokService
{
    private readonly IHistorijaIzmjeneService _historijaIzmjeneService;
    private readonly IObrokRepository _obrokRepository;

    public ObrokService(IObrokRepository obrokRepository, IHistorijaIzmjeneService historijaIzmjeneService)
    {
        _obrokRepository = obrokRepository;
        _historijaIzmjeneService = historijaIzmjeneService;
    }

    public Task<List<Obrok>> GetAllAsync()
    {
        return _obrokRepository.GetAllAsync();
    }

    public Task<List<Obrok>> GetAvailableAsync()
    {
        return _obrokRepository.GetAvailableAsync();
    }

    public Task<Obrok?> GetByIdAsync(int? id)
    {
        return id is null ? Task.FromResult<Obrok?>(null) : _obrokRepository.GetByIdAsync(id.Value);
    }

    public async Task CreateAsync(Obrok obrok)
    {
        await _obrokRepository.AddAsync(obrok);
        await _obrokRepository.SaveChangesAsync();
        await _historijaIzmjeneService.ZabiljeziAsync(nameof(Obrok), obrok.Id, "Kreiranje", $"Kreiran obrok {obrok.Naziv}.");
    }

    public async Task<bool> UpdateAsync(int id, Obrok obrok)
    {
        if (id != obrok.Id)
        {
            return false;
        }

        _obrokRepository.Update(obrok);
        await _obrokRepository.SaveChangesAsync();
        await _historijaIzmjeneService.ZabiljeziAsync(nameof(Obrok), obrok.Id, "Izmjena", $"Izmijenjen obrok {obrok.Naziv}.");
        return true;
    }

    public Task<Obrok?> DeletePreviewAsync(int? id)
    {
        return GetByIdAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        var obrok = await _obrokRepository.GetByIdAsync(id);
        if (obrok is null)
        {
            return;
        }

        _obrokRepository.Remove(obrok);
        await _obrokRepository.SaveChangesAsync();
        await _historijaIzmjeneService.ZabiljeziAsync(nameof(Obrok), obrok.Id, "Brisanje", $"Obrisan obrok {obrok.Naziv}.");
    }

    public Task<bool> ExistsAsync(int id)
    {
        return _obrokRepository.ExistsAsync(id);
    }

    public Task<int> CountAsync()
    {
        return _obrokRepository.CountAsync();
    }

    public Task<int> CountAvailableAsync()
    {
        return _obrokRepository.CountAvailableAsync();
    }
}
