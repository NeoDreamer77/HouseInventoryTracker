using System;
using System.IO;
using HouseInventoryTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HouseInventoryTracker.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Item> Items { get; set; } = null!;

    private readonly string _dbPath;

    public AppDbContext()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appFolder = Path.Combine(appDataPath, "HouseInventoryTracker");
        Directory.CreateDirectory(appFolder);
        _dbPath = Path.Combine(appFolder, "inventory.db");
    }

    public AppDbContext(string dbPath)
    {
        _dbPath = dbPath;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={_dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.PhotoPath).HasMaxLength(500);
            entity.Property(e => e.PurchasePrice).HasPrecision(18, 2);

            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.Location);
            entity.HasIndex(e => e.Name);
        });
    }
}
