using CampusEats.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Data;

public class CampusEatsUserStore :
    IUserPasswordStore<Korisnik>,
    IUserEmailStore<Korisnik>,
    IUserRoleStore<Korisnik>
{
    private readonly ApplicationDbContext _context;

    public CampusEatsUserStore(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Dispose()
    {
    }

    public async Task<IdentityResult> CreateAsync(Korisnik user, CancellationToken cancellationToken)
    {
        _context.Korisnici.Add(user);
        await _context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(Korisnik user, CancellationToken cancellationToken)
    {
        _context.Korisnici.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public Task<Korisnik?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        return _context.Korisnici.FirstOrDefaultAsync(user => user.NormalizedEmail == normalizedEmail, cancellationToken);
    }

    public Task<Korisnik?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        return int.TryParse(userId, out var id)
            ? _context.Korisnici.FirstOrDefaultAsync(user => user.Id == id, cancellationToken)
            : Task.FromResult<Korisnik?>(null);
    }

    public Task<Korisnik?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        return _context.Korisnici.FirstOrDefaultAsync(user => user.NormalizedUserName == normalizedUserName, cancellationToken);
    }

    public Task<string?> GetEmailAsync(Korisnik user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(Korisnik user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.EmailConfirmed);
    }

    public Task<string?> GetNormalizedEmailAsync(Korisnik user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedEmail);
    }

    public Task<string?> GetNormalizedUserNameAsync(Korisnik user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedUserName);
    }

    public Task<string?> GetPasswordHashAsync(Korisnik user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PasswordHash);
    }

    public Task<string> GetUserIdAsync(Korisnik user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Id.ToString());
    }

    public Task<string?> GetUserNameAsync(Korisnik user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.UserName);
    }

    public Task<bool> HasPasswordAsync(Korisnik user, CancellationToken cancellationToken)
    {
        return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
    }

    public Task SetEmailAsync(Korisnik user, string? email, CancellationToken cancellationToken)
    {
        user.Email = email;
        return Task.CompletedTask;
    }

    public Task SetEmailConfirmedAsync(Korisnik user, bool confirmed, CancellationToken cancellationToken)
    {
        user.EmailConfirmed = confirmed;
        return Task.CompletedTask;
    }

    public Task SetNormalizedEmailAsync(Korisnik user, string? normalizedEmail, CancellationToken cancellationToken)
    {
        user.NormalizedEmail = normalizedEmail;
        return Task.CompletedTask;
    }

    public Task SetNormalizedUserNameAsync(Korisnik user, string? normalizedName, CancellationToken cancellationToken)
    {
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public Task SetPasswordHashAsync(Korisnik user, string? passwordHash, CancellationToken cancellationToken)
    {
        user.PasswordHash = passwordHash;
        return Task.CompletedTask;
    }

    public Task SetUserNameAsync(Korisnik user, string? userName, CancellationToken cancellationToken)
    {
        user.UserName = userName;
        return Task.CompletedTask;
    }

    public async Task<IdentityResult> UpdateAsync(Korisnik user, CancellationToken cancellationToken)
    {
        _context.Korisnici.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public async Task AddToRoleAsync(Korisnik user, string roleName, CancellationToken cancellationToken)
    {
        var role = await _context.IdentityRoles.FirstOrDefaultAsync(r => r.NormalizedName == roleName, cancellationToken);
        if (role is null)
        {
            return;
        }

        var exists = await _context.IdentityUserRoles.AnyAsync(
            userRole => userRole.UserId == user.Id && userRole.RoleId == role.Id,
            cancellationToken);

        if (!exists)
        {
            _context.IdentityUserRoles.Add(new IdentityUserRole<int> { UserId = user.Id, RoleId = role.Id });
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IList<string>> GetRolesAsync(Korisnik user, CancellationToken cancellationToken)
    {
        return await _context.IdentityUserRoles
            .Where(userRole => userRole.UserId == user.Id)
            .Join(_context.IdentityRoles, userRole => userRole.RoleId, role => role.Id, (userRole, role) => role.Name!)
            .ToListAsync(cancellationToken);
    }

    public async Task<IList<Korisnik>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        var role = await _context.IdentityRoles.FirstOrDefaultAsync(r => r.NormalizedName == roleName, cancellationToken);
        if (role is null)
        {
            return new List<Korisnik>();
        }

        return await _context.IdentityUserRoles
            .Where(userRole => userRole.RoleId == role.Id)
            .Join(_context.Korisnici, userRole => userRole.UserId, user => user.Id, (userRole, user) => user)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsInRoleAsync(Korisnik user, string roleName, CancellationToken cancellationToken)
    {
        var role = await _context.IdentityRoles.FirstOrDefaultAsync(r => r.NormalizedName == roleName, cancellationToken);
        return role is not null && await _context.IdentityUserRoles.AnyAsync(
            userRole => userRole.UserId == user.Id && userRole.RoleId == role.Id,
            cancellationToken);
    }

    public async Task RemoveFromRoleAsync(Korisnik user, string roleName, CancellationToken cancellationToken)
    {
        var role = await _context.IdentityRoles.FirstOrDefaultAsync(r => r.NormalizedName == roleName, cancellationToken);
        if (role is null)
        {
            return;
        }

        var userRole = await _context.IdentityUserRoles.FirstOrDefaultAsync(
            item => item.UserId == user.Id && item.RoleId == role.Id,
            cancellationToken);

        if (userRole is not null)
        {
            _context.IdentityUserRoles.Remove(userRole);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
