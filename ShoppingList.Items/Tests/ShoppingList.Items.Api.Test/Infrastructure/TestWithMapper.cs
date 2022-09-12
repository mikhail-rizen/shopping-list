using AutoMapper;
using ShoppingList.Items.Api.Infrastructure;

namespace ShoppingList.Items.Api.Test.Infrastructure
{
    public class TestWithMapper
    {
        protected readonly IMapper mapper;

        public TestWithMapper()
        {
            var mockMapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            mapper = mockMapperCfg.CreateMapper();
        }
    }
}
