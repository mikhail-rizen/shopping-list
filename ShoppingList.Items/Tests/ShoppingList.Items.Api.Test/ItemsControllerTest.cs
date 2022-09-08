using MediatR;
using ShoppingList.Items.Api.Controllers;
using ShoppingList.Items.Api.Test.Infrastructure;
using ShoppingList.Items.Data.Entities;
using ShoppingList.Items.Service.Query;

namespace ShoppingList.Items.Api.Test
{
    public class ItemsControllerTest : TestWithMapper
    {
        ItemsController controller;

        public ItemsControllerTest()
        {
            IMediator mediator = A.Fake<IMediator>();
            A.CallTo(() => mediator.Send(A<GetItemByIdQuery>._, A<CancellationToken>._)).Returns(new Item
            {
                Id = Guid.NewGuid(),
                Name = "some name"
            });

            A.CallTo(() => mediator.Send(A<GetAllItemsQuery>._, A<CancellationToken>._)).Returns(Task.FromResult((IEnumerable<Item>)new Item[]
            {
                new Item
                {
                    Id = Guid.NewGuid(),
                    Name = "some name"
                }
            }));

            controller = new ItemsController(mediator, mapper);
        }

        [Fact]
        public async void TestGet()
        {
            (await controller.Get(CancellationToken.None)).First().Name.Should().Be("some name");
        }
    }
}