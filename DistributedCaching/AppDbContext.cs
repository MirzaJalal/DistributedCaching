using DistributedCaching.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace DistributedCaching
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet to represent the Products table
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        base.OnModelCreating(modelBuilder);

        // Seeding 100 sample products
        var sampleProducts = new List<Product>();
        for (int i = 1; i <= 1000; i++)
        {
                var random = new Random(); // Declare and initialize Random

                decimal randomPrice = Math.Round((decimal)(random.NextDouble() * 99) + 1, 2);

                sampleProducts.Add(new Product(
                $"Product {i}",
                $"Description of Product {i}",
                randomPrice
            ));
        }

            modelBuilder.Entity<Product>().HasData(sampleProducts);


            // Configure the Product entity
    modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id); // Primary key
                entity.Property(p => p.Name)
                      .IsRequired()
                      .HasMaxLength(100); // Name is required and has a max length
                entity.Property(p => p.Description)
                      .HasMaxLength(500); // Description max length
                entity.Property(p => p.Price)
                      .HasPrecision(18, 2); // Precision for decimal values
            });
        }
    }
}
