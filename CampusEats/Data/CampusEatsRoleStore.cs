using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Data;

public class CampusEatsRoleStore : IRoleStore<IdentityRole<int>>
{
    private readonly ApplicationDbContext _context;

    public CampusEatsRoleStore(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Dispose()
    {
    }

    public async Task<IdentityResult> CreateAsync(IdentityRole<int> role, CancellationToken cancellationToken)
    {
        _context.IdentityRoles.Add(role);
        await _context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(IdentityRole<int> role, CancellationToken cancellationToken)
    {
        _context.IdentityRoles.Remove(role);
        await _context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public Task<IdentityRole<int>?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        return int.TryParse(roleId, out var id)
            ? _context.IdentityRoles.FirstOrDefaultAsync(role => role.Id == id, cancellationToken)
            : Task.FromResult<IdentityRole<int>?>(null);
    }

    public Task<IdentityRole<int>?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        return _context.IdentityRoles.FirstOrDefaultAsync(role => role.NormalizedName == normalizedRoleName, cancellationToken);
    }

    public Task<string?> GetNormalizedRoleNameAsync(IdentityRole<int> role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.NormalizedName);
    }

    public Task<string> GetRoleIdAsync(IdentityRole<int> role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.Id.ToString());
    }

    public Task<string?> GetRoleNameAsync(IdentityRole<int> role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.Name);
    }

    public Task SetNormalizedRoleNameAsync(IdentityRole<int> role, string? normalizedName, CancellationToken cancellationToken)
    {
        role.NormalizedName = normalizedName;
        return Task.CompletedTask;
    }

    public Task SetRoleNameAsync(IdentityRole<int> role, string? roleName, CancellationToken cancellationToken)
    {
        role.Name = roleName;
        return Task.CompletedTask;
    }

    public async Task<IdentityResult> UpdateAsync(IdentityRole<int> role, CancellationToken cancellationToken)
    {
        _context.IdentityRoles.Update(role);
        await _context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }
}
