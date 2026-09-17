using IdeaTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdeaTracker.Infrastructure.Persistence
{
    public class IdeaTrackerDbContext(DbContextOptions<IdeaTrackerDbContext> options) : DbContext(options)
    {
        public DbSet<Idea> Ideas => Set<Idea>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Idea>(e =>
            {
                e.ToTable("Ideas");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).UseIdentityColumn(1, 1);
                e.Property(x => x.Title).IsRequired().HasMaxLength(200);
                e.Property(x => x.Description).HasMaxLength(4000);
                e.Property(x => x.Status).HasConversion<string>().HasMaxLength(20).IsRequired();

                e.Property(x => x.Tags)
                    .HasConversion(
                        tags => string.Join(',', tags),
                        csv => string.IsNullOrWhiteSpace(csv)
                            ? new List<string>()
                            : csv.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
                    .HasMaxLength(500)
                    .Metadata.SetValueComparer(new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<List<string>>(
                        (a, b) => (a ?? new()).SequenceEqual(b ?? new()),
                        v => v.Aggregate(0, (hash, s) => HashCode.Combine(hash, s.GetHashCode())),
                        v => v.ToList()));

                e.Property(x => x.CreatedAt).IsRequired();
                e.Property(x => x.UpdatedAt).IsRequired();

                e.HasIndex(x => x.Status);
                e.HasIndex(x => x.CreatedAt);
            });
        }
    }
}
