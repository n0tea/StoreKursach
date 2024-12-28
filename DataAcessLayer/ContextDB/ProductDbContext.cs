using DataAcessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAcessLayer.ContextDB
{
    public class ProductDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");
                entity.HasKey(p => p.Id);
                entity.HasIndex(p => p.Uid).IsUnique();

                entity.Property(p => p.Name).HasMaxLength(256).IsRequired();
                entity.Property(p => p.Description).HasMaxLength(1024).IsRequired(false);
                entity.Property(p => p.Size).HasMaxLength(50).IsRequired(false);
            });
        }
    }
}
