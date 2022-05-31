using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingList.Items.Data.Database;
using ShoppingList.Items.Data.Entities;

namespace ShoppingList.Items.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly ItemsContext itemsContext;

        public ItemsController(ItemsContext itemsContext)
        {
            this.itemsContext = itemsContext;
        }

        [HttpGet]
        public async Task<IEnumerable<Item>> Get()
        {
            return await itemsContext.Items.ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post(string name)
        {
            Item item = new Item { Id = Guid.NewGuid(), Name = name };
            await itemsContext.AddAsync(item);
            return Ok();
        }
    }
}