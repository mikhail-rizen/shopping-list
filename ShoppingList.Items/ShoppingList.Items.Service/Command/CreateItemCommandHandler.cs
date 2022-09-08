using MediatR;
using ShoppingList.Items.Data.Entities;
using ShoppingList.Items.Data.Repository;

namespace ShoppingList.Items.Service.Command
{
    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, Item>
    {
        private readonly IItemsRepository itemsRepository;

        public CreateItemCommandHandler(IItemsRepository itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }

        public async Task<Item> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            return await itemsRepository.AddAsync(request.Item);
        }
    }
}
