using ShoppingList.Items.Entities;

namespace ShoppingList.Items.Messaging.Send
{
    public interface ISyncItemSender
    {
        Task SendSyncItem(Item item, CancellationToken cancellationToken);
    }
}
