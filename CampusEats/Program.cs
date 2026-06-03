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

// Repositories and services
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

// create roles
using (var rolesScope = app.Services.CreateScope())
{
    var roleManager = rolesScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = { "Student", "Radnik", "Dostavljac", "Admin" };
    foreach (var r in roles)
    {
        if (!await roleManager.RoleExistsAsync(r))
        {
            await roleManager.CreateAsync(new IdentityRole(r));
        }
    }
}

// seed users
using (var usersScope = app.Services.CreateScope())
{
    var services = usersScope.ServiceProvider;
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

            if (!await userManager.IsInRoleAsync(user, role))
                await userManager.AddToRoleAsync(user, role);

            try
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var reset = await userManager.ResetPasswordAsync(user, token, password);
                if (!reset.Succeeded)
                {
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

// Ensure Obrok table has Category and ImageUrl columns and seed sample menu if empty
using (var schemaScope = app.Services.CreateScope())
{
    var services = schemaScope.ServiceProvider;
    var db = services.GetRequiredService<DataContext>();
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        using var conn = (DbConnection)db.Database.GetDbConnection();
        await conn.OpenAsync();

        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Obroci' AND COLUMN_NAME = 'Category'";
            var catCol = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            if (catCol == 0)
            {
                try
                {
                    using var alter = conn.CreateCommand();
                    alter.CommandText = "ALTER TABLE Obroci ADD Category nvarchar(100) NOT NULL DEFAULT 'Meals'";
                    await alter.ExecuteNonQueryAsync();
                    logger.LogInformation("Added Obroci.Category column.");
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Failed adding Category column to Obroci.");
                }
            }

            cmd.CommandText = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Obroci' AND COLUMN_NAME = 'ImageUrl'";
            var imgCol = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            if (imgCol == 0)
            {
                try
                {
                    using var alter = conn.CreateCommand();
                    alter.CommandText = "ALTER TABLE Obroci ADD ImageUrl nvarchar(400) NOT NULL DEFAULT ''";
                    await alter.ExecuteNonQueryAsync();
                    logger.LogInformation("Added Obroci.ImageUrl column.");
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Failed adding ImageUrl column to Obroci.");
                }
            }

            // seed sample items if table empty
            cmd.CommandText = "SELECT COUNT(*) FROM Obroci";
            var count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            if (count == 0)
            {
                logger.LogInformation("Seeding sample Obroci items.");
                var items = new[] {
                    new Obrok { Naziv = "Steak with Potatoes", Opis = "Tender steak with roasted potatoes.", Cijena = 5.50, Sastojci = "Beef, Potatoes, Salt", Dostupan = true, Category = "Meals", ImageUrl = "https://images.unsplash.com/photo-1551183053-bf91a1d81141" },
                    new Obrok { Naziv = "Pasta Carbonara", Opis = "Classic carbonara with bacon and cheese.", Cijena = 6.00, Sastojci = "Pasta, Eggs, Bacon", Dostupan = true, Category = "Meals", ImageUrl = "https://images.unsplash.com/photo-1523986371872-9d3ba2e2f642" },
                    new Obrok { Naziv = "Burger and Fries", Opis = "Juicy burger served with crispy fries.", Cijena = 5.00, Sastojci = "Beef, Bun, Potatoes", Dostupan = true, Category = "Meals", ImageUrl = "https://images.unsplash.com/photo-1550547660-d9450f859349" },
                    new Obrok { Naziv = "Chicken with Vegetables", Opis = "Grilled chicken with seasonal vegetables.", Cijena = 5.50, Sastojci = "Chicken, Vegetables", Dostupan = true, Category = "Meals", ImageUrl = "https://images.unsplash.com/photo-1604908177522-48a6e8b9f2d9" },
                    new Obrok { Naziv = "Orange Juice", Opis = "Freshly squeezed orange juice.", Cijena = 3.00, Sastojci = "Oranges", Dostupan = true, Category = "Drinks", ImageUrl = "https://images.unsplash.com/photo-1544025162-d76694265947" },
                    new Obrok { Naziv = "Sparkling Water", Opis = "Chilled sparkling water.", Cijena = 2.00, Sastojci = "Water", Dostupan = true, Category = "Drinks", ImageUrl = "https://images.unsplash.com/photo-1582719478250-66acbaf0dbd2" },
                    new Obrok { Naziv = "Cola", Opis = "Classic cola drink.", Cijena = 2.50, Sastojci = "Carbonated Water, Sugar", Dostupan = true, Category = "Drinks", ImageUrl = "https://images.unsplash.com/photo-1585386959984-a415522c11f6" },
                    new Obrok { Naziv = "Coffee", Opis = "Hot brewed coffee.", Cijena = 2.00, Sastojci = "Coffee Beans, Water", Dostupan = true, Category = "Warm Drinks", ImageUrl = "https://images.unsplash.com/photo-1509042239860-f550ce710b93" },
                    new Obrok { Naziv = "Tea", Opis = "Warm herbal tea.", Cijena = 1.50, Sastojci = "Tea Leaves, Water", Dostupan = true, Category = "Warm Drinks", ImageUrl = "https://images.unsplash.com/photo-1504639725590-34d0984388bd" },
                    new Obrok { Naziv = "Hot Chocolate", Opis = "Creamy hot chocolate.", Cijena = 2.50, Sastojci = "Cocoa, Milk, Sugar", Dostupan = true, Category = "Warm Drinks", ImageUrl = "https://images.unsplash.com/photo-1547592166-5e9b6f3c4d5f" }
                };

                foreach (var it in items)
                {
                    try
                    {
                        db.Obroci.Add(it);
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(ex, "Failed creating sample Obrok: {Name}", it.Naziv);
                    }
                }
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Failed saving seeded Obroci items.");
                }
            }
        }
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "Obrok schema/seed check skipped due to error.");
    }
}

// Runtime data migration: safely migrate Korisnici -> AspNetUsers using UserManager and parameterized SQL.
using (var migrationScope = app.Services.CreateScope())
{
    var services = migrationScope.ServiceProvider;
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

