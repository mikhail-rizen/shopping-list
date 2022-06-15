using Microsoft.AspNetCore.Mvc;
using ShoppingList.Items.Data.Entities;
using ShoppingList.Items.Data.Repository;

namespace ShoppingList.Items.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly ItemsRepository itemsRepository;

        public ItemsController(ItemsRepository itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Item>> Get()
        {
            return await itemsRepository.GetAllAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post(string name)
        {
            Item item = new Item { Id = Guid.NewGuid(), Name = name };
            await itemsRepository.AddAsync(item);
            return Ok();
        }
    }
}