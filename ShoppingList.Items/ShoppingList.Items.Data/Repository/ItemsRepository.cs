using Microsoft.Extensions.Logging;
using ShoppingList.Items.Data.Database;
using ShoppingList.Items.Data.Entities;

namespace ShoppingList.Items.Data.Repository
{
    public class ItemsRepository : Repository<Item>
    {
        private readonly ItemsContext itemsContext;
        private readonly ILogger<ItemsRepository> logger;

        public ItemsRepository(ItemsContext itemsContext, ILogger<ItemsRepository> logger) : base(itemsContext, logger)
        {
            this.itemsContext = itemsContext;
            this.logger = logger;
        }

        public async Task<Item?> GetById(Guid id)
        {
            try
            {
                return await itemsContext.Items.FindAsync(id);
            }
            catch(Exception e)
            {
                logger.LogError(e, "Cannot retrieve item by id");
                throw new Exception($"Cannot retrieve item by id: {e.Message}");
            }
        }
    }
}
