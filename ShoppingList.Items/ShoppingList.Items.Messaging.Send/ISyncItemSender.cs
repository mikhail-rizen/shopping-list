using ShoppingList.Items.Data.Entities;

namespace ShoppingList.Items.Messaging.Send
{
    public interface ISyncItemSender
    {
        Task SendSyncItem(Item item, CancellationToken cancellationToken);
    }
}
