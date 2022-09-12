using ShoppingList.Items.Entities;

namespace ShoppingList.Items.Data.Repository
{
    public interface IItemsRepository : IRepository<Item>
    {
        Task<Item?> GetById(Guid id);
    }
}
