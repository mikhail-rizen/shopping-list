using MediatR;
using ShoppingList.Items.Data.Entities;

namespace ShoppingList.Items.Service.Query
{
    public class GetAllItemsQuery : IRequest<IEnumerable<Item>>
    {
    }
}
