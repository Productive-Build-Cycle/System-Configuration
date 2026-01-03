using Microsoft.EntityFrameworkCore;
using PBC.SystemConfiguration.Domain.Entities;

namespace PBC.SystemConfiguration.Infrastructure.Persistence.DbContext;

public class ProgramDbContext(DbContextOptions<ProgramDbContext> options) : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public DbSet<FeatureFlag> FeatureFlags { get; set; }
    public DbSet<AppSetting> AppSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureFeatureFlag(modelBuilder);
        ConfigureAppSetting(modelBuilder);
        ConfigureBaseEntity(modelBuilder);

        SeedData(modelBuilder);
    }

    private static void ConfigureFeatureFlag(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FeatureFlag>(entity =>
        {
            entity.ToTable("FeatureFlags");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(x => x.Name)
                .IsUnique();

            entity.Property(x => x.Description)
                .HasMaxLength(500);

            entity.Property(x => x.IsEnabled)
                .IsRequired();
        });
    }

    private static void ConfigureAppSetting(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppSetting>(entity =>
        {
            entity.ToTable("AppSettings");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Key)
                .IsRequired()
                .HasMaxLength(200);

            entity.HasIndex(x => x.Key)
                .IsUnique();

            entity.Property(x => x.Value)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(x => x.Type)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(x => x.Description)
                .HasMaxLength(500);
        });
    }

    private static void ConfigureBaseEntity(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.Id))
                    .ValueGeneratedOnAdd();

                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.CreatedAt))
                    .HasDefaultValueSql("GETUTCDATE()");
            }
        }
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FeatureFlag>().HasData(
            new FeatureFlag
            {
                Id = 1,
                Name = "New UI",
                Description = "Enables new UI",
                IsEnabled = false,
                CreatedAt = new DateTime(2026, 1, 1)
            }
        );
    }
}