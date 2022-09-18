using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShoppingList.Items.Api.Models;
using ShoppingList.Items.Entities;
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
        private readonly ILogger<ItemsController> logger;

        public ItemsController(IMediator mediator, IMapper mapper, ILogger<ItemsController> logger)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemModel>> Get(CancellationToken cancellationToken)
        {
            logger.LogInformation("ItemsController.Get");
            return mapper.Map<IEnumerable<ItemModel>>(await mediator.Send(new GetAllItemsQuery(), cancellationToken));
        }

        [HttpGet("{id}")]
        public async Task<ItemModel> GetById(Guid id, CancellationToken cancellationToken)
        {
            logger.LogInformation("ItemsController.GetById ({id})", id);
            return mapper.Map<ItemModel>(await mediator.Send(new GetItemByIdQuery { Id = id }, cancellationToken));
        }

        [HttpPost]
        public async Task<ActionResult<ItemModel>> Post(CreateItemModel createItemModel, CancellationToken cancellationToken)
        {
            logger.LogInformation("ItemsController.Post ({@model})", createItemModel);
            Item item = await mediator.Send(new CreateItemCommand { Item = mapper.Map<Item>(createItemModel) }, cancellationToken);
            return Ok(mapper.Map<ItemModel>(item));
        }
    }
}