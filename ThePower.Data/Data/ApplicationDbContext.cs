using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ThePower.Models;

namespace ThePowerData.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Membership> Memberships { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set;}
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
