using MediatR;
using ShoppingList.Items.Data.Entities;

namespace ShoppingList.Items.Service.Command
{
    public class CreateItemCommand : IRequest<Item>
    {
        public Item Item { get; init; } = null!;
    }
}
