using Mango.Services.AuthAPI;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.AuthAPI.Data
{
    public class AppDBContext :IdentityDbContext<ApplicationUser>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
            
        }
       public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        //public override int SaveChanges()
        //{
        //    AppliedChanges();
        //    return base.SaveChanges();
        //}

        //public void AppliedChanges()
        //{
        //    var entries = ChangeTracker.Entries().Where(e => e.Entity is Coupon && (e.State == EntityState.Modified));
        //    foreach (var entry in entries)
        //    {
        //        if (entry.Entity is Coupon coupon)
        //        {
        //            coupon.LastUpdated = DateTime.Now;
        //        }
        //    }
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
        }
    }
}
