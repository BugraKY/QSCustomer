using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace QSCustomer.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options){}
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }

    public class SecondDbContext : DbContext
    {
        public SecondDbContext(DbContextOptions<SecondDbContext> options) 
            : base(options){}

        public DbSet<musteriYetkili> musteriYetkili { get; set; }
        public DbSet<musteritanim> musteritanim { get; set; }
        public DbSet<qprojetanim> qprojetanim { get; set; }
        public DbSet<qprojeDetay> qprojedetay { get; set; }
        public DbSet<qprojeDetays> qprojedetays { get; set; }
        public DbSet<fabrikatanim> fabrikatanim { get; set; }
        public DbSet<qprojedurumu> qprojedurumu { get; set; }
        public DbSet<qprojecontroltipi> qprojecontroltipi { get; set; }
        public DbSet<qprojepartNrTanimi> qprojepartNrTanimi { get; set; }
        public DbSet<qprojehataTanimi> qprojehataTanimi { get; set; }
        public DbSet<qprojeHataDetay> qprojeHataDetay { get; set; }
        public DbSet<paraBirimi> paraBirimi { get; set; }
        public DbSet<ulke> ulke { get; set; }
    }
}
