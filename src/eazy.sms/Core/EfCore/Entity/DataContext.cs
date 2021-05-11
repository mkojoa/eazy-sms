using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace eazy.sms.Core.EfCore.Entity
{
    public sealed class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
            if (!Database.CanConnect()) Database.EnsureCreated();
        }

        public static DbSet<EventMessage> EventMessages => null;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventMessage>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Message).HasDefaultValueSql("(NULL)");
                entity.Property(e => e.ExceptionStatus).HasDefaultValueSql("(NULL)");
                entity.Property(e => e.Exceptions).HasDefaultValueSql("(NULL)");
                entity.Property(e => e.Status).HasDefaultValueSql("(0)");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(x => x.UpdatedAt).HasDefaultValueSql("(getdate())").ValueGeneratedOnAddOrUpdate();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        private static void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Model.AddAnnotation("", "");
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity) entityEntry.Entity).UpdatedAt = DateTime.Now;

                if (entityEntry.State == EntityState.Added) ((BaseEntity) entityEntry.Entity).CreatedAt = DateTime.Now;
            }

            return base.SaveChanges();
        }
    }
}