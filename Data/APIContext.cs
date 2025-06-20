using Eletronic_Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Eletronic_Api.Data
{
    public class APIContext : DbContext
    {

        public APIContext(DbContextOptions<APIContext> options) : base(options) { }

        public DbSet<Customers> Customers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
      public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<OtpStore> OtpStores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
       
            modelBuilder.Entity<Item>()
           .HasOne(s => s.Category)
           .WithMany(g => g.Items)
           .HasForeignKey(s => s.CategoryID);
            modelBuilder.Entity<Item>()
                .HasOne(s => s.Brand)
                .WithMany(g => g.Items)
                .HasForeignKey(s => s.BrandID);
            modelBuilder.Entity<Item>()
                .HasOne(s => s.Promotion)
                .WithMany(g => g.Items)
                .HasForeignKey(s => s.PromotionID);
        }
    }
}
