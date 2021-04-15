using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace eazy.sms.Core.EfCore.Entity
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
            if (!Database.CanConnect()) Database.EnsureCreated(); 
        }

        public virtual DbSet<EventMessage> EventMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventMessage>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Message).HasDefaultValueSql("(NULL)");
                entity.Property(e => e.Status).HasDefaultValueSql("(false)");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(NULL)");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(NULL)");
            });

        }
    }
}
