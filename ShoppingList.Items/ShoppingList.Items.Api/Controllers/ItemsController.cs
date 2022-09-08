using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShoppingList.Items.Api.Models;
using ShoppingList.Items.Data.Entities;
using ShoppingList.Items.Service.Command;
using ShoppingList.Items.Service.Query;

namespace ShoppingList.Items.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public ItemsController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemModel>> Get(CancellationToken cancellationToken)
        {
            return mapper.Map<IEnumerable<ItemModel>>(await mediator.Send(new GetAllItemsQuery(), cancellationToken));
        }

        [HttpGet("{id}")]
        public async Task<ItemModel> GetById(Guid id, CancellationToken cancellationToken)
        {
            return mapper.Map<ItemModel>(await mediator.Send(new GetItemByIdQuery { Id = id }, cancellationToken));
        }

        [HttpPost]
        public async Task<ActionResult<ItemModel>> Post(CreateItemModel createItemModel, CancellationToken cancellationToken)
        {
            Item item = await mediator.Send(new CreateItemCommand { Item = mapper.Map<Item>(createItemModel) }, cancellationToken);
            return Ok(mapper.Map<ItemModel>(item));
        }
    }
}