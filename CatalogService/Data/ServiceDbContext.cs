using CatalogService.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Data
{
    public class ServiceDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }

        public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasOne(c => c.Catalog)
                .WithMany(i => i.Items)
                .HasForeignKey(c => c.CatalogId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
