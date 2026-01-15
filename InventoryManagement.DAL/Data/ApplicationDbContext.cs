using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.Web.Models;

namespace InventoryManagement.Web.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Supplier>(entity =>
        {
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        });

        builder.Entity<Product>(entity =>
        {
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            entity.HasOne(e => e.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(e => e.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(e => e.ProductName);
            entity.HasIndex(e => e.Category);
        });
    }
}
