using GSM_Operator_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GSM_Operator_Project.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Musteri> Musteriler { get; set; }
        public DbSet<Tarife> Tarifeler { get; set; }
        public DbSet<MusteriTarife> MusteriTarifeleri { get; set; }
        public DbSet<Fatura> Faturalar { get; set; }
        public DbSet<Tahsilat> Tahsilatlar { get; set; }
        public DbSet<Rol> Roller { get; set; }
        public DbSet<KullaniciRol> KullaniciRoller { get; set; }

    }
}
