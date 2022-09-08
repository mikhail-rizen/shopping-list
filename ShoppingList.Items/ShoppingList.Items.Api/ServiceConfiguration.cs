using ShoppingList.Items.Data.Repository;
using MediatR;
using ShoppingList.Items.Service.Query;

namespace ShoppingList.Items.Api
{
    public class ServiceConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IItemsRepository, ItemsRepository>();
            services.AddAutoMapper(typeof(ServiceConfiguration));
            services.AddMediatR(typeof(GetAllItemsQuery).Assembly);
        }
    }
}
