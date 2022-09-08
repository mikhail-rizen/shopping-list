using MediatR;
using ShoppingList.Items.Data.Entities;

namespace ShoppingList.Items.Service.Query
{
    public class GetItemByIdQuery : IRequest<Item?>
    {
        public Guid Id { get; init; }
    }
}
