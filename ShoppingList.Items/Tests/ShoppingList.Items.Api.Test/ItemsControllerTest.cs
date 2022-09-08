using ShoppingList.Items.Api.Controllers;
using ShoppingList.Items.Api.Test.Infrastructure;
using ShoppingList.Items.Data.Entities;
using ShoppingList.Items.Data.Repository;

namespace ShoppingList.Items.Api.Test
{
    public class ItemsControllerTest : TestWithMapper
    {
        ItemsController controller;
        
        public ItemsControllerTest()
        {
            IItemsRepository repository = A.Fake<IItemsRepository>();
            A.CallTo(() => repository.GetById(A<Guid>._)).Returns(new Item
            {
                Id = Guid.NewGuid(),
                Name = "some name"
            });

            A.CallTo(() => repository.GetAllAsync()).Returns(Task.FromResult((IEnumerable<Item>)new Item[]
            {
                new Item
                {
                    Id = Guid.NewGuid(),
                    Name = "some name"
                }
            }));

            controller = new ItemsController(repository, mapper);
        }

        [Fact]
        public async void TestGet()
        {
            (await controller.Get()).First().Name.Should().Be("some name");
        }
    }
}