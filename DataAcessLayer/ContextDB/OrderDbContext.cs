using DataAcessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAcessLayer.ContextDB
{
    public class OrderDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public OrderDbContext(DbContextOptions<OrderDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("order");
                entity.HasKey(o => o.Id);
                entity.HasIndex(o => o.Uid).IsUnique();

                entity.Property(o => o.CreationTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("orderitem");
                entity.HasKey(oi => oi.Id);
                entity.HasIndex(oi => oi.Uid).IsUnique();
                entity.HasOne<Order>()
                      .WithMany()
                      .HasForeignKey(oi => oi.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Связь с продуктами
                entity.Property(oi => oi.Price).IsRequired();
            });

        }
    }
}
