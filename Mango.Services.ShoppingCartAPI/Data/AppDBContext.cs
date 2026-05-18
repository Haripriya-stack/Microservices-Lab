using Mango.Services.ShoppingCartAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.Data
{
    public class AppDBContext :DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
            
        }
        public DbSet<CartHeader> CartHeaders { get; set; }
        public DbSet<CartDetails> CartDetails   { get; set; }
        public override int SaveChanges()
        {
            AppliedChanges();
            return base.SaveChanges();
        }

        public void AppliedChanges()
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is CartHeader && ((e.State == EntityState.Modified) || (e.State == EntityState.Added)));
            foreach (var entry in entries)
            {
                if (entry.Entity is CartHeader product)
                {
                    product.LastUpdated = DateTime.Now;
                }
            }
        }
        
        
    }
}
