using ShoppingList.Items.Data.Repository;

namespace ShoppingList.Items.Api
{
    public class ServiceConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ItemsRepository>();
            services.AddAutoMapper(typeof(ServiceConfiguration));
        }
    }
}
