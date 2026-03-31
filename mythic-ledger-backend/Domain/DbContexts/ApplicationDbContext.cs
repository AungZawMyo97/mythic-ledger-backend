using Microsoft.EntityFrameworkCore;
using mythic_ledger_backend.Domain.Entities;

namespace mythic_ledger_backend.Domain.DbContexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderType> OrderTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<UserRole>("public", "UserRole");

        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<Customer>().ToTable("Customer");
        modelBuilder.Entity<Order>().ToTable("Order");
        modelBuilder.Entity<OrderType>().ToTable("OrderType");

        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasColumnType("UserRole");

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}
