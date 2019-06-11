using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tenli.Server.Data.Models;
using Tenli.Server.Services;

namespace Tenli.Server.Data {
  public class ApplicationDbContext : DbContext {
    private readonly DbContextService _dbContextService;
    public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options, DbContextService dbContextService) : base (options) {
      _dbContextService = dbContextService;
    }

    protected override void OnModelCreating (ModelBuilder modelBuilder) {
      modelBuilder.HasPostgresExtension ("postgis");

      #region Configures one-to-many relationship
      //Orders to User
      modelBuilder.Entity<Order> ()
        .HasOne<ApplicationUser> (s => s.Executor)
        .WithMany (g => g.DeliveredOrders)
        .HasForeignKey (s => s.ExecutorId);

      modelBuilder.Entity<Order> ()
        .HasOne<ApplicationUser> (s => s.Customer)
        .WithMany (g => g.ReceivedOrders)
        .HasForeignKey (s => s.CustomerId);

      #endregion Configures one-to-many relationship
      #region Configures many-to-many relationship
      // UserRole 
      modelBuilder.Entity<UserRole> ()
        .HasKey (ur => new { ur.ApplicationRoleId, ur.ApplicationUserId });

      modelBuilder.Entity<UserRole> ()
        .HasOne (ur => ur.ApplicationUser)
        .WithMany (u => u.UserRoles)
        .HasForeignKey (ur => ur.ApplicationUserId);

      modelBuilder.Entity<UserRole> ()
        .HasOne (ur => ur.ApplicationRole)
        .WithMany (r => r.UserRoles)
        .HasForeignKey (ur => ur.ApplicationRoleId);
      //UserRoles

      //OrderProduct
      modelBuilder.Entity<OrderProduct> ()
        .HasKey (op => new { op.OrderId, op.ProductId });

      modelBuilder.Entity<OrderProduct> ()
        .HasOne (op => op.Order)
        .WithMany (o => o.OrderProducts)
        .HasForeignKey (op => op.OrderId);

      modelBuilder.Entity<OrderProduct> ()
        .HasOne (op => op.Product)
        .WithMany (p => p.OrderProducts)
        .HasForeignKey (op => op.ProductId);
      //OrderProducts

      #endregion Configures many-to-many relationship

      modelBuilder.Entity<Culture> ()
        .HasIndex (c => new { c.Key })
        .IsUnique (true);

      modelBuilder.Entity<OrderStatus> ()
        .HasIndex (os => new { os.Key })
        .IsUnique (true);

      modelBuilder.Entity<ProductType> ()
        .HasIndex (pt => new { pt.Key })
        .IsUnique (true);

      modelBuilder.Entity<ApplicationUser> ()
        .HasIndex (os => new { os.Email })
        .IsUnique (true);

      modelBuilder.Entity<Order> ().Property (o => o.StartLocation)
        .ForSqliteHasSrid (4326);

      modelBuilder.Entity<Order> ().Property (o => o.EndLocation)
        .ForSqliteHasSrid (4326);

      modelBuilder.HasPostgresExtension ("postgis");

      base.OnModelCreating (modelBuilder);
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<ActiveSession> ActiveSessions { get; set; }
    public DbSet<ApplicationRole> ApplicationRoles { get; set; }
    public DbSet<Culture> Cultures { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderStatus> OrderStatuses { get; set; }
    public DbSet<LocalizationResource> LocalizationResources { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<DeliveryType> DeliveryTypes { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<WeightUnit> WeightUnits { get; set; }
    public DbSet<VolumeUnit> VolumeUnits { get; set; }
    public DbSet<SizeUnit> SizeUnits { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }

    public override int SaveChanges () {
      _dbContextService.AddBaseEntityProperties (ChangeTracker);

      var result = base.SaveChanges ();
      return result;
    }
    public override async Task<int> SaveChangesAsync (bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default (CancellationToken)) {
      _dbContextService.AddBaseEntityProperties (ChangeTracker);

      int result = await base.SaveChangesAsync (acceptAllChangesOnSuccess, cancellationToken);
      return result;
    }

    public override async Task<int> SaveChangesAsync (CancellationToken cancellationToken = default (CancellationToken)) {
      _dbContextService.AddBaseEntityProperties (ChangeTracker);

      int result = await base.SaveChangesAsync (cancellationToken);
      return result;
    }
  }
}