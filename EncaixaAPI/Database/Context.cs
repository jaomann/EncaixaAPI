using EncaixaAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EncaixaAPI.Database
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Box> Boxes { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
            });

            modelBuilder.Entity<Box>(entity =>
            {
                entity.ToTable("Boxes");

                entity.HasKey(b => b.Id);

                entity.Property(b => b.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(b => b.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(b => b.Width)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(b => b.Height)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(b => b.Depth)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(b => b.AvailableQuantity)
                    .IsRequired();

                entity.Property(b => b.MaxWeight)
                    .HasColumnType("decimal(10,2)")
                    .HasDefaultValue(10m);

                entity.Property(b => b.Type)
                    .HasMaxLength(50)
                    .HasDefaultValue("Padrão");

                entity.Property(b => b.IsActive)
                    .HasDefaultValue(true);

                entity.Ignore(b => b.Volume);

                entity.HasIndex(b => new { b.Width, b.Height, b.Depth })
                    .HasDatabaseName("IX_Box_Dimensions");

                entity.HasIndex(b => b.Name)
                    .IsUnique()
                    .HasDatabaseName("IX_Box_Name");
            });

            if (Database.IsSqlServer())
            {
                modelBuilder.HasDefaultSchema("dbo");
            }
        }
    }
}