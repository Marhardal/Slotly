using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Slotly.Server.Seeds;

namespace Slotly.Server;

public class DBContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public DBContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<ServiceModel> Services { get; set; }

    public DbSet<ServiceAddOnsModel> ServiceAddOns { get; set; }

    public DbSet<BookingModel> Bookings { get; set; }

    public DbSet<CategoryModel> Categories { get; set; }

    public DbSet<StatusModel> Statuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BookingModel>()
        .HasOne(b => b.Service)
        .WithMany(b => b.Bookings)
        .HasForeignKey(b => b.ServiceID)
        .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<StatusModel>()
        .HasMany(b => b.Bookings)
        .WithOne(s => s.Status)
        .HasForeignKey(b => b.statusID)
        .OnDelete(DeleteBehavior.Cascade);  

        modelBuilder.Entity<CategoryModel>()
        .HasMany(s => s.Services)
        .WithOne(s => s.Category)
        .HasForeignKey(s => s.CategoryID)
        .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<ServiceModel>()
        .HasMany(s => s.serviceAddOns)
        .WithOne(s => s.Service)
        .HasForeignKey(s => s.ServiceID)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CategoryModel>().HasData(
            CategorySeed.GetSeedData()
        );

        modelBuilder.Entity<StatusModel>().HasData(
            StatusSeed.GetSeedData()
        );
    }

}
