using MediatR;
using ShoppingList.Items.Entities;
using ShoppingList.Items.Data.Repository;
using ShoppingList.Items.Messaging.Send;

namespace ShoppingList.Items.Service.Command
{
    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, Item>
    {
        private readonly IItemsRepository itemsRepository;
        private readonly ISyncItemSender syncItemSender;

        public CreateItemCommandHandler(IItemsRepository itemsRepository, ISyncItemSender syncItemSender)
        {
            this.itemsRepository = itemsRepository;
            this.syncItemSender = syncItemSender;
        }

        public async Task<Item> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            Item item = await itemsRepository.AddAsync(request.Item);
            await syncItemSender.SendSyncItem(item, cancellationToken);
            return item;
        }
    }
}
