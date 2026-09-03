using CardRequestApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CardRequestApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<CardRequest> CardRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CardRequest>()
            .HasIndex(cr => cr.ReferenceNumber)
            .IsUnique();

        modelBuilder.Entity<CardRequest>()
            .Property(cr => cr.Status)
            .HasConversion<string>();

        modelBuilder.Entity<CardRequest>()
            .Property(cr => cr.CardType)
            .HasConversion<string>();
    }
}