using Microsoft.EntityFrameworkCore;
using System;
using TNAI_Proj.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Car> Cars { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Dealership> Dealerships { get; set; }
    public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Car configuration
        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.Horsepower).HasPrecision(18, 2);
            entity.Property(e => e.Torque).HasPrecision(18, 2);
            entity.Property(e => e.TopSpeed).HasPrecision(18, 2);
            entity.Property(e => e.Acceleration).HasPrecision(18, 2);
            entity.Property(e => e.FuelEfficiency).HasPrecision(18, 2);
            entity.Property(e => e.FuelType).IsRequired();
            entity.Property(e => e.Transmission).IsRequired();

            entity.HasOne(e => e.Category)
                .WithMany(c => c.Cars)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.User)
                .WithMany(u => u.BoughtCars)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Dealership)
                .WithMany(d => d.Cars)
                .HasForeignKey(e => e.DealershipId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Category configuration
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).IsRequired();
        });

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Role).IsRequired();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Order configuration
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.TransactionId).HasMaxLength(100);
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Car)
                .WithMany(c => c.Orders)
                .HasForeignKey(e => e.CarId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Review configuration
        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Rating).IsRequired();
            entity.Property(e => e.Comment).IsRequired().HasMaxLength(1000);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Car)
                .WithMany(c => c.Reviews)
                .HasForeignKey(e => e.CarId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Dealership configuration
        modelBuilder.Entity<Dealership>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Address).IsRequired().HasMaxLength(200);
            entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.ManagerName).HasMaxLength(100);
        });

        // MaintenanceRecord configuration
        modelBuilder.Entity<MaintenanceRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ServiceType).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.Cost).HasPrecision(18, 2);
            entity.Property(e => e.ServiceProvider).HasMaxLength(100);

            entity.HasOne(e => e.Car)
                .WithMany(c => c.MaintenanceRecords)
                .HasForeignKey(e => e.CarId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
} 