using Microsoft.EntityFrameworkCore;
using ShortUrlGen.Data.Models;

namespace ShortUrlGen.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<UrlMapping> UrlMappings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UrlMapping>(entity =>
            {
                entity.Property(e => e.LongUrl)
                    .IsRequired()
                    .HasMaxLength(2048);
                entity.Property(e => e.ShortUrl)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }
    }
}
