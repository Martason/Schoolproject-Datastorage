using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Backend.Data
{
    public class EpicFoodRescueDBContext : DbContext
    {
        public EpicFoodRescueDBContext(DbContextOptions options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Foodbox> Foodboxes { get; set; }
        public DbSet<UserPrivateInfo> UserPrivateInfos { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
            });

            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.HasIndex(e => e.CompanyName).IsUnique();
                entity.HasIndex(e => e.Organisationsnummer).IsUnique();
            });

            modelBuilder.Entity<UserPrivateInfo>(entity =>
            {
                entity.HasIndex(e => e.Username).IsUnique();

            });
        }



    }
}
