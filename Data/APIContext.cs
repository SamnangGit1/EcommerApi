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
 
     
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<OtpStore> OtpStores { get; set; }

        public DbSet<Item> Items { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
 


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Item>()
              .HasOne(i => i.Category)
              .WithMany(c => c.Items)
              .HasForeignKey(i => i.CategoryID)
              .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<Item>()
                .HasOne(i => i.Brand)
                .WithMany(b => b.Items)
                .HasForeignKey(i => i.BrandID)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
