using MediatR;
using ShoppingList.Items.Data.Entities;
using ShoppingList.Items.Service.Command;
using ShoppingList.Items.Service.Model;
using ShoppingList.Items.Service.Query;

namespace ShoppingList.Items.Service.Service
{
    public class SyncItemService : ISyncItemService
    {
        private readonly IMediator mediator;

        public SyncItemService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task SyncItem(SyncItemModel syncItem, CancellationToken cancellationToken)
        {
            Item? item = await mediator.Send(new GetItemByIdQuery { Id = syncItem.Id }, cancellationToken).ConfigureAwait(false);
            if (item != default)
            {
                //already exists
                return;
            }
            await mediator.Send(new CreateItemCommand { Item = new Item { Id = syncItem.Id, Name = syncItem.Name } }, cancellationToken).ConfigureAwait(false);
        }
    }
}
