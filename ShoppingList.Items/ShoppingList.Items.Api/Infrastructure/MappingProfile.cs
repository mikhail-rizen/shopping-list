using AutoMapper;
using ShoppingList.Items.Api.Models;
using ShoppingList.Items.Entities;

namespace ShoppingList.Items.Api.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Item, ItemModel>();
            CreateMap<CreateItemModel, Item>().ForMember(x => x.Id, opt => opt.MapFrom((_) => Guid.NewGuid()));
        }
    }
}
