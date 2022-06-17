using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShoppingList.Items.Api.Models;
using ShoppingList.Items.Data.Entities;
using ShoppingList.Items.Data.Repository;

namespace ShoppingList.Items.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly ItemsRepository itemsRepository;
        private readonly IMapper mapper;

        public ItemsController(ItemsRepository itemsRepository, IMapper mapper)
        {
            this.itemsRepository = itemsRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemModel>> Get()
        {
            return mapper.Map<IEnumerable<ItemModel>>(await itemsRepository.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateItemModel createItemModel)
        {
            await itemsRepository.AddAsync(mapper.Map<Item>(createItemModel));
            return Ok();
        }
    }
}