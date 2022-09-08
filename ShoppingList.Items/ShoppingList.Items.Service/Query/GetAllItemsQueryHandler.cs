using MediatR;
using ShoppingList.Items.Data.Entities;
using ShoppingList.Items.Data.Repository;

namespace ShoppingList.Items.Service.Query
{
    public class GetAllItemsQueryHandler : IRequestHandler<GetAllItemsQuery, IEnumerable<Item>>
    {
        private readonly IItemsRepository itemsRepository;

        public GetAllItemsQueryHandler(IItemsRepository itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }

        public async Task<IEnumerable<Item>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
        {
            return await itemsRepository.GetAllAsync();
        }
    }
}
