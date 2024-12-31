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

                entity.Property(o => o.UserId).IsRequired();

                entity.Property(o => o.CreationTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");


                entity.HasMany(o => o.OrderItems)
                      .WithOne(oi => oi.Order)
                      .HasForeignKey(oi => oi.OrderId)
                      .OnDelete(DeleteBehavior.Cascade); 
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("orderitem");
                entity.HasKey(oi => oi.Id);
                entity.HasIndex(oi => oi.Uid).IsUnique();

                entity.Property(oi => oi.ProductId).IsRequired();

                entity.HasOne(oi => oi.Order)
                      .WithMany(o => o.OrderItems)
                      .HasForeignKey(oi => oi.OrderId);

                entity.Property(oi => oi.Price).IsRequired();
            });

        }
    }
}
