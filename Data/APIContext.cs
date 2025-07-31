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
        public DbSet<User> Users { get; set; }
        public DbSet<Userpermission> UserPermissions { get; set; }
        public DbSet<Staff> staffs { get; set; }
        public DbSet<ItemCard> ItemCards { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

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
            modelBuilder.Entity<Userpermission>()
                .HasOne(up => up.Users)
                .WithMany(u => u.UserPermissions)
                .HasForeignKey(up => up.UserID)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ItemCard>()
              .HasOne(ic => ic.Item)
              .WithMany(i => i.ItemCards)
              .HasForeignKey(ic => ic.ItemID)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemCard>()
                .HasOne(ic => ic.AppUser)
                .WithMany(p => p.ItemCards)
                .HasForeignKey(ic => ic.AppUserID);
            modelBuilder.Entity<OrderDetail>()
               .HasOne(od => od.Item)
               .WithMany(i => i.OrderDetails)
               .HasForeignKey(od => od.ItemID)
               .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderID)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.AppUser)
                .WithMany(a => a.orders)
                .HasForeignKey(o => o.AppUserID)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Staff)
                .WithMany(s => s.orders)
                .HasForeignKey(o => o.StuffID)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
