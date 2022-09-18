using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingList.Items.Entities;

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

            EntityTypeBuilder<Item> itemBuilder = modelBuilder.Entity<Item>();
            itemBuilder.Property(item => item.Name).HasMaxLength(200);
            itemBuilder.HasIndex(item => item.Name, "IX_Items_Name");
        }

        public DbSet<Item> Items { get; set; } = null!;
    }
}
