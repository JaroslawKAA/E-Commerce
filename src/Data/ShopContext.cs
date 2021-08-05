using System;
using Codecool.CodecoolShop.Models;
using Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class ShopContext: IdentityDbContext<User>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Order> Orders { get; set; }
        
        public DbSet<Cart> Carts { get; set; }
        public DbSet<BillingAddressModel> BillingAddresses { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var connString = configuration.GetConnectionString("DefaultConnection");
            
            optionsBuilder
                .UseSqlServer(connString)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .Property(m => m.Id)
                .IsRequired();
            modelBuilder.Entity<Order>()
                .HasOne(o => o.BillingAddressModel);
            modelBuilder.Entity<Order>()
                .HasMany<ProductEntityView>();

            modelBuilder.Entity<User>()
                .Navigation(x => x.BillingAddress);

            modelBuilder.Entity<BillingAddressModel>()
                .HasOne<User>();
            base.OnModelCreating(modelBuilder);
        }
    }
}