using ShoppingList.Items.Service.Model;

namespace ShoppingList.Items.Service.Service
{
    public interface ISyncItemService
    {
        Task SyncItem(SyncItemModel syncItem, CancellationToken cancellationToken);
    }
}
