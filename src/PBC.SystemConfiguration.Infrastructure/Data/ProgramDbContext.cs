using PBC.SystemConfiguration.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ProgramDbContext : DbContext
{
    public ProgramDbContext(DbContextOptions<ProgramDbContext> options)
        : base(options)
    {
    }

    public DbSet<FeatureFlag> FeatureFlags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FeatureFlag>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                  .ValueGeneratedOnAdd();

            entity.Property(x => x.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.HasIndex(x => x.Name)
                  .IsUnique();

            entity.Property(x => x.Description)
                  .HasMaxLength(500);

            entity.Property(x => x.CreateDate)
                  .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(x => x.LastUpdateDate)
                  .HasDefaultValueSql("GETUTCDATE()");
        });
    }
    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<FeatureFlag>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastUpdateDate = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Added)
            {
                entry.Entity.CreateDate = DateTime.UtcNow;
                entry.Entity.LastUpdateDate = DateTime.UtcNow;
            }
        }

        return base.SaveChanges();
    }
}

