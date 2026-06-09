using CampusEats.Models;
using Microsoft.AspNetCore.Identity;

namespace CampusEats.Data;

public static class IdentitySeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Korisnik>>();

        foreach (var roleName in Enum.GetNames<UlogaKorisnika>())
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole<int>(roleName));
            }
        }

        await CreateUserAsync(userManager, "amina@student.ba", "student123", "Amina", "Hodzic", UlogaKorisnika.Student, "19001", "061111222", "Studentski dom Nedzarici");
        await CreateUserAsync(userManager, "radnik@campuseats.ba", "radnik123", "Tarik", "Basic", UlogaKorisnika.RadnikMenze, null, "062333444", null);
        await CreateUserAsync(userManager, "admin@campuseats.ba", "admin123", "Lejla", "Admin", UlogaKorisnika.Administrator, null, null, null);
        await CreateUserAsync(userManager, "kurir@campuseats.ba", "kurir123", "Emir", "Kurir", UlogaKorisnika.Kurir, null, "063555666", null);
    }

    private static async Task CreateUserAsync(
        UserManager<Korisnik> userManager,
        string email,
        string password,
        string ime,
        string prezime,
        UlogaKorisnika uloga,
        string? brojIndeksa,
        string? telefon,
        string? adresa)
    {
        var existingUser = await userManager.FindByEmailAsync(email);
        if (existingUser is not null)
        {
            existingUser.Ime = ime;
            existingUser.Prezime = prezime;
            existingUser.Uloga = uloga;
            existingUser.BrojIndeksa = brojIndeksa;
            existingUser.Telefon = telefon;
            existingUser.PhoneNumber = telefon;
            existingUser.Adresa = adresa;
            await userManager.UpdateAsync(existingUser);

            var currentRoles = await userManager.GetRolesAsync(existingUser);
            if (currentRoles.Count > 0)
            {
                await userManager.RemoveFromRolesAsync(existingUser, currentRoles);
            }

            await userManager.AddToRoleAsync(existingUser, uloga.ToString());
            return;
        }

        var user = new Korisnik
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            Ime = ime,
            Prezime = prezime,
            Uloga = uloga,
            BrojIndeksa = brojIndeksa,
            Telefon = telefon,
            PhoneNumber = telefon,
            Adresa = adresa
        };

        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, uloga.ToString());
        }
    }
}
