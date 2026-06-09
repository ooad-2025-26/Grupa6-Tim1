using CampusEats.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Korisnik> Korisnici => Set<Korisnik>();
    public DbSet<IdentityRole<int>> IdentityRoles => Set<IdentityRole<int>>();
    public DbSet<IdentityUserRole<int>> IdentityUserRoles => Set<IdentityUserRole<int>>();
    public DbSet<Obrok> Obroci => Set<Obrok>();
    public DbSet<Meni> Meniji => Set<Meni>();
    public DbSet<Rezervacija> Rezervacije => Set<Rezervacija>();
    public DbSet<QRKod> QRKodovi => Set<QRKod>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Korisnik>(entity =>
        {
            entity.ToTable("Korisnici");
            entity.HasIndex(korisnik => korisnik.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();
            entity.HasIndex(korisnik => korisnik.NormalizedEmail).HasDatabaseName("EmailIndex");
        });

        modelBuilder.Entity<IdentityRole<int>>(entity =>
        {
            entity.ToTable("IdentityRoles");
            entity.HasKey(role => role.Id);
            entity.HasIndex(role => role.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();
        });

        modelBuilder.Entity<IdentityUserRole<int>>(entity =>
        {
            entity.ToTable("IdentityUserRoles");
            entity.HasKey(userRole => new { userRole.UserId, userRole.RoleId });
        });

        modelBuilder.Entity<Rezervacija>()
            .HasOne(rezervacija => rezervacija.QRKod)
            .WithOne(qrKod => qrKod.Rezervacija)
            .HasForeignKey<QRKod>(qrKod => qrKod.RezervacijaId);

        modelBuilder.Entity<Obrok>().HasData(
            new Obrok { Id = 1, Naziv = "Pileci file sa rizom", Cijena = 5.50m, Opis = "Topli studentski obrok sa prilogom i salatom.", Sastojci = "Piletina, riza, salata", Dostupan = true, Kolicina = 30 },
            new Obrok { Id = 2, Naziv = "Vegetarijanska pasta", Cijena = 4.80m, Opis = "Pasta sa povrcem i paradajz sosom.", Sastojci = "Pasta, tikvice, paprika, paradajz", Dostupan = true, Kolicina = 18 },
            new Obrok { Id = 3, Naziv = "Corba i domace pecivo", Cijena = 3.20m, Opis = "Lagani dnevni obrok za pauzu izmedju predavanja.", Sastojci = "Povrce, zacini, pecivo", Dostupan = true, Kolicina = 24 }
        );

        modelBuilder.Entity<Meni>().HasData(
            new Meni { Id = 1, Datum = new DateTime(2026, 6, 1), ObrokId = 1 },
            new Meni { Id = 2, Datum = new DateTime(2026, 6, 1), ObrokId = 2 },
            new Meni { Id = 3, Datum = new DateTime(2026, 6, 2), ObrokId = 3 }
        );
    }
}
