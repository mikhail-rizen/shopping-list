using Microsoft.EntityFrameworkCore;
using ShoppingList.Items.Data.Entities;

namespace ShoppingList.Items.Data.Database
{
    public class ItemsContext : DbContext
    {
        public ItemsContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Items");

            modelBuilder.Entity<Item>();
        }

        public DbSet<Item> Items { get; set; } = null!;
    }
}
