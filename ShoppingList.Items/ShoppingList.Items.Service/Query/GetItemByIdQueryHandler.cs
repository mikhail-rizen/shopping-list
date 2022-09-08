using MediatR;
using ShoppingList.Items.Data.Entities;
using ShoppingList.Items.Data.Repository;

namespace ShoppingList.Items.Service.Query
{
    public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, Item?>
    {
        private readonly IItemsRepository itemsRepository;

        public GetItemByIdQueryHandler(IItemsRepository itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }

        public async Task<Item?> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
        {
            return await itemsRepository.GetById(request.Id);
        }
    }
}
