using CampusEats.Models;
using CampusEats.Repositories;

namespace CampusEats.Services;

public class MeniService : IMeniService
{
    private readonly IHistorijaIzmjeneService _historijaIzmjeneService;
    private readonly IMeniRepository _meniRepository;

    public MeniService(IMeniRepository meniRepository, IHistorijaIzmjeneService historijaIzmjeneService)
    {
        _meniRepository = meniRepository;
        _historijaIzmjeneService = historijaIzmjeneService;
    }

    public Task<List<Meni>> GetAllAsync()
    {
        return _meniRepository.GetAllAsync();
    }

    public Task<Meni?> GetByIdAsync(int? id)
    {
        return id is null ? Task.FromResult<Meni?>(null) : _meniRepository.GetByIdAsync(id.Value);
    }

    public Task<Meni?> GetByIdWithObrokAsync(int? id)
    {
        return id is null ? Task.FromResult<Meni?>(null) : _meniRepository.GetByIdWithObrokAsync(id.Value);
    }

    public async Task CreateAsync(Meni meni)
    {
        await _meniRepository.AddAsync(meni);
        await _meniRepository.SaveChangesAsync();
        await _historijaIzmjeneService.ZabiljeziAsync(nameof(Meni), meni.Id, "Kreiranje", $"Kreiran meni za datum {meni.Datum:dd.MM.yyyy}.");
    }

    public async Task<bool> UpdateAsync(int id, Meni meni)
    {
        if (id != meni.Id)
        {
            return false;
        }

        _meniRepository.Update(meni);
        await _meniRepository.SaveChangesAsync();
        await _historijaIzmjeneService.ZabiljeziAsync(nameof(Meni), meni.Id, "Izmjena", $"Izmijenjen meni za datum {meni.Datum:dd.MM.yyyy}.");
        return true;
    }

    public async Task DeleteAsync(int id)
    {
        var meni = await _meniRepository.GetByIdAsync(id);
        if (meni is null)
        {
            return;
        }

        _meniRepository.Remove(meni);
        await _meniRepository.SaveChangesAsync();
        await _historijaIzmjeneService.ZabiljeziAsync(nameof(Meni), meni.Id, "Brisanje", $"Obrisan meni za datum {meni.Datum:dd.MM.yyyy}.");
    }

    public Task<bool> ExistsAsync(int id)
    {
        return _meniRepository.ExistsAsync(id);
    }
}
