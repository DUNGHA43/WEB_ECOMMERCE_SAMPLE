using Microsoft.EntityFrameworkCore;
using WebEcommerce.Models;

namespace WebEcommerce.Service
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}
