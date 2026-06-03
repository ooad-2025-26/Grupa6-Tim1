using CampusEats.Data;
using CampusEats.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Microsoft.Data.SqlClient;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

// Register Obrok repository and service
builder.Services.AddScoped<CampusEats.Interfaces.IObrokRepository, CampusEats.Repositories.ObrokRepository>();
builder.Services.AddScoped<CampusEats.Interfaces.IObrokService, CampusEats.Services.ObrokService>();
builder.Services.AddScoped<CampusEats.Interfaces.IRezervacijaRepository, CampusEats.Repositories.RezervacijaRepository>();
builder.Services.AddScoped<CampusEats.Interfaces.IRezervacijaService, CampusEats.Services.RezervacijaService>();
builder.Services.AddScoped<CampusEats.Interfaces.IMeniRepository, CampusEats.Repositories.MeniRepository>();
builder.Services.AddScoped<CampusEats.Interfaces.IMeniService, CampusEats.Services.MeniService>();
builder.Services.AddScoped<CampusEats.Interfaces.IQRKodRepository, CampusEats.Repositories.QRKodRepository>();
builder.Services.AddScoped<CampusEats.Interfaces.IQRKodService, CampusEats.Services.QRKodService>();
builder.Services.AddScoped<CampusEats.Interfaces.IDostavaRepository, CampusEats.Repositories.DostavaRepository>();
builder.Services.AddScoped<CampusEats.Interfaces.IDostavaService, CampusEats.Services.DostavaService>();
builder.Services.AddScoped<CampusEats.Interfaces.IZalihaRepository, CampusEats.Repositories.ZalihaRepository>();
builder.Services.AddScoped<CampusEats.Interfaces.IZalihaService, CampusEats.Services.ZalihaService>();

var app = builder.Build();

using (var scope= app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = { "Student", "Radnik", "Dostavljac", "Admin" };

    foreach(var r in roles)
    {
        if(!await roleManager.RoleExistsAsync(r))
        {
            await roleManager.CreateAsync(new IdentityRole(r));
        }
    }
}

// Seed demo users for each role if they do not exist
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var logger = services.GetRequiredService<ILogger<Program>>();

    async Task EnsureUser(string email, string password, string role, string first = "Demo", string last = "User")
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                Ime = first,
                Prezime = last,
                BrojIndeksa = role == "Student" ? "2025/000" : "N/A",
                Adresa = "N/A"
            };
            var create = await userManager.CreateAsync(user, password);
            if (create.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
                await userManager.AddToRoleAsync(user, role);
                logger.LogInformation("Created demo user {Email} with role {Role}", email, role);
            }
            else
            {
                logger.LogError("Failed creating demo user {Email}: {Errors}", email, string.Join(';', create.Errors.Select(e => e.Description)));
            }
        }
        else
        {
            // update required fields and ensure email confirmed
            var updated = false;
            if (user.Ime != first) { user.Ime = first; updated = true; }
            if (user.Prezime != last) { user.Prezime = last; updated = true; }
            if (string.IsNullOrEmpty(user.BrojIndeksa) && role == "Student") { user.BrojIndeksa = "2025/000"; updated = true; }
            if (string.IsNullOrEmpty(user.Adresa)) { user.Adresa = "N/A"; updated = true; }
            if (!user.EmailConfirmed) { user.EmailConfirmed = true; updated = true; }
            if (updated)
            {
                var upRes = await userManager.UpdateAsync(user);
                if (!upRes.Succeeded)
                    logger.LogWarning("Failed updating demo user {Email}: {Errors}", email, string.Join(';', upRes.Errors.Select(e => e.Description)));
            }

            // ensure role assigned
            if (!await userManager.IsInRoleAsync(user, role))
                await userManager.AddToRoleAsync(user, role);

            // reset the password to the expected demo password
            try
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var reset = await userManager.ResetPasswordAsync(user, token, password);
                if (!reset.Succeeded)
                {
                    // fallback: if user has a password, remove it then try to add; otherwise try AddPasswordAsync
                    if (await userManager.HasPasswordAsync(user))
                    {
                        var remove = await userManager.RemovePasswordAsync(user);
                        if (remove.Succeeded)
                        {
                            var add = await userManager.AddPasswordAsync(user, password);
                            if (!add.Succeeded)
                            {
                                logger.LogError("Failed setting demo password for {Email}: {Errors}", email, string.Join(';', add.Errors.Select(e => e.Description)));
                            }
                        }
                        else
                        {
                            logger.LogError("Failed removing existing password for {Email}: {Errors}", email, string.Join(';', remove.Errors.Select(e => e.Description)));
                        }
                    }
                    else
                    {
                        var add = await userManager.AddPasswordAsync(user, password);
                        if (!add.Succeeded)
                        {
                            logger.LogError("Failed adding demo password for {Email}: {Errors}", email, string.Join(';', add.Errors.Select(e => e.Description)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception while resetting password for demo user {Email}", email);
            }
        }
    }

    await EnsureUser("student@campuseats.com", "Student123!", "Student", "Student", "User");
    await EnsureUser("radnik@campuseats.com", "Radnik123!", "Radnik", "Worker", "User");
    await EnsureUser("dostavljac@campuseats.com", "Dostavljac123!", "Dostavljac", "Delivery", "User");
    await EnsureUser("admin@campuseats.com", "Admin123!", "Admin", "Admin", "User");
}

// Runtime data migration: safely migrate Korisnici -> AspNetUsers using UserManager and parameterized SQL.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        using var conn = (DbConnection)db.Database.GetDbConnection();
        await conn.OpenAsync();

        // check if Korisnici table exists
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Korisnici'";
            var tblCount = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            if (tblCount == 0)
            {
                logger.LogInformation("Korisnici table not present; skipping runtime migration.");
            }
            else
            {
                // check if any Rezervacije rows still need migration (KorisnikId is null)
                cmd.CommandText = "SELECT COUNT(*) FROM Rezervacije WHERE KorisnikId IS NULL";
                var need = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                if (need == 0)
                {
                    logger.LogInformation("No Rezervacije require migration.");
                }
                else
                {
                    // read all Korisnici rows
                    cmd.CommandText = "SELECT Id, Ime, Prezime, Email, BrojIndeksa, Adresa FROM Korisnici";
                    using var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        var oldId = reader.GetInt32(0);
                        var ime = reader.IsDBNull(1) ? null : reader.GetString(1);
                        var prezime = reader.IsDBNull(2) ? null : reader.GetString(2);
                        var email = reader.IsDBNull(3) ? null : reader.GetString(3);
                        var broj = reader.IsDBNull(4) ? null : reader.GetString(4);
                        var adresa = reader.IsDBNull(5) ? null : reader.GetString(5);

                        try
                        {
                            ApplicationUser user = null;
                            if (!string.IsNullOrEmpty(email))
                                user = await userManager.FindByEmailAsync(email);

                            if (user == null)
                            {
                                user = new ApplicationUser
                                {
                                    Ime = ime ?? string.Empty,
                                    Prezime = prezime ?? string.Empty,
                                    Email = email,
                                    UserName = email ?? ("user_" + Guid.NewGuid().ToString("N")),
                                    BrojIndeksa = broj ?? string.Empty,
                                    Adresa = adresa ?? string.Empty,
                                    EmailConfirmed = false
                                };

                                // create user with a random temporary password
                                var tempPassword = "Tmp@" + Guid.NewGuid().ToString("N").Substring(0, 12);
                                var createResult = await userManager.CreateAsync(user, tempPassword);
                                if (!createResult.Succeeded)
                                {
                                    logger.LogError("Failed to create user for Korisnici Id {OldId}: {Errors}", oldId, string.Join(';', createResult.Errors.Select(e => e.Description)));
                                    continue;
                                }
                                logger.LogInformation("Created ApplicationUser {UserId} for Korisnici {OldId}", user.Id, oldId);
                            }

                            // ensure role
                            if (!await userManager.IsInRoleAsync(user, "Student"))
                                await userManager.AddToRoleAsync(user, "Student");

                            // update Rezervacije rows referencing the old id
                            using var updateCmd = conn.CreateCommand();
                            updateCmd.CommandText = "UPDATE Rezervacije SET KorisnikId = @newId WHERE KorisnikIdInt = @oldId";
                            var pNew = updateCmd.CreateParameter(); pNew.ParameterName = "@newId"; pNew.Value = user.Id; updateCmd.Parameters.Add(pNew);
                            var pOld = updateCmd.CreateParameter(); pOld.ParameterName = "@oldId"; pOld.Value = oldId; updateCmd.Parameters.Add(pOld);
                            var updated = await updateCmd.ExecuteNonQueryAsync();
                            logger.LogInformation("Updated {Count} Rezervacije for Korisnici {OldId}", updated, oldId);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "Error migrating Korisnici Id {OldId}", oldId);
                        }
                    }

                    // try to enforce non-null and drop the old int column; log but don't fail startup on error
                    try
                    {
                        using var alterCmd = conn.CreateCommand();
                        alterCmd.CommandText = "ALTER TABLE Rezervacije ALTER COLUMN KorisnikId nvarchar(450) NOT NULL";
                        await alterCmd.ExecuteNonQueryAsync();
                        using var dropCmd = conn.CreateCommand();
                        dropCmd.CommandText = "ALTER TABLE Rezervacije DROP COLUMN KorisnikIdInt";
                        await dropCmd.ExecuteNonQueryAsync();
                        logger.LogInformation("Converted Rezervacije.KorisnikId to non-null and dropped KorisnikIdInt");
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(ex, "Post-migration schema cleanup failed; please inspect the database and run manual cleanup if needed.");
                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "Runtime migration skipped due to error; run migration manually on a DB copy.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
