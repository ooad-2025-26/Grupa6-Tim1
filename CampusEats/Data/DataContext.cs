using CampusEats.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Obrok> Obroci { get; set; }
        public DbSet<Rezervacija> Rezervacije { get; set; }
        public DbSet<Meni> Meniji { get; set; }
        public DbSet<QRKod> QRKodovi { get; set; }
        public DbSet<Dostava> Dostave { get; set; }
        public DbSet<Zaliha> Zalihe { get; set; }
    }
}