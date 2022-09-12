using MediatR;
using ShoppingList.Items.Entities;

namespace ShoppingList.Items.Service.Query
{
    public class GetAllItemsQuery : IRequest<IEnumerable<Item>>
    {
    }
}
