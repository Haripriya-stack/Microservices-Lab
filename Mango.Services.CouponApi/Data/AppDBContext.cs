using Mango.Services.CouponApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponApi.Data
{
    public class AppDBContext :DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
            
        }
        public DbSet<Coupon> Coupons { get; set; }

        public override int SaveChanges()
        {
            AppliedChanges();
            return base.SaveChanges();
        }

        public void AppliedChanges()
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is Coupon && (e.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                if (entry.Entity is Coupon coupon)
                {
                    coupon.LastUpdated = DateTime.Now;
                }
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Coupon>().HasData(new Coupon()
            {
                CouponId = 1,
                CouponCode = "SAVE10",
                DiscountAmount =10,
                MinAmount =20
            
            });
            modelBuilder.Entity<Coupon>().HasData(new Coupon()
            {
                CouponId = 2,
                CouponCode = "SAVE40",
                DiscountAmount = 40,
                MinAmount = 60
            
            });
            modelBuilder.Entity<Coupon>().HasData(new Coupon()
            {
                CouponId = 3,
                CouponCode = "SAVE20",
                DiscountAmount = 20,
                MinAmount = 40
              
            });

            modelBuilder.Entity<Coupon>()
              .Property(x => x.LastUpdated)
               .HasDefaultValueSql("GETUTCDATE()")
               .ValueGeneratedOnAddOrUpdate();
        }
    }
}
