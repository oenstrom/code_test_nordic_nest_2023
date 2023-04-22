using Microsoft.EntityFrameworkCore;

namespace src.Data;

public class ApplicationDbContext : DbContext {
  public DbSet<Models.Price> Price { get; set; } = default!;

  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
  
  /**
   * Set the precision and scale of the decimal type.
   */
  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.Entity<Models.Price>(entity => {
      entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,6)");
    });
  }
}