using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using ProductManager.Domain;

namespace ProductManager.Data
{
    class ApplicationDbContext : DbContext
    {
        private string connectionString =
            "Server=.;Database=ProductManager;Integrated Security=true;Encrypt=False";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.LogTo(message => Debug.WriteLine(message));
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
