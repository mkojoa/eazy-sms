using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace eazy.sms.Core.EfCore.Entity
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
            try
            {
                if (!Database.CanConnect()) Database.EnsureCreated();
            }
            catch (Exception)
            { }
        }

        public DbSet<EventMessage> EventMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            modelBuilder.Entity<EventMessage>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Message).HasDefaultValueSql("(NULL)");
                entity.Property(
                    e => e.ResultStatus).HasDefaultValueSql("(NULL)");
                entity.Property(e => e.ResultMessage).HasDefaultValueSql("(NULL)");
                entity.Property(e => e.SentStatus).HasDefaultValueSql("(0)");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(x => x.UpdatedAt).HasDefaultValueSql("(getdate())").ValueGeneratedOnAddOrUpdate();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        private static void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}