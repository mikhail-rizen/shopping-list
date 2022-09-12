using MediatR;
using ShoppingList.Items.Data.Repository;
using ShoppingList.Items.Messaging.Configuration;
using ShoppingList.Items.Messaging.Receive;
using ShoppingList.Items.Messaging.Send;
using ShoppingList.Items.Service.Query;
using ShoppingList.Items.Service.Service;

namespace ShoppingList.Items.Api
{
    public class ServiceConfiguration
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IItemsRepository, ItemsRepository>();
            services.AddAutoMapper(typeof(ServiceConfiguration));
            services.AddMediatR(typeof(GetAllItemsQuery).Assembly);

            services.AddScoped<ISyncItemService, SyncItemService>();

            //services.AddTransient<IRequestHandler<GetItemByIdQuery, Item?>, GetItemByIdQueryHandler>();

            //messaging
            var serviceClientSettingsConfig = configuration.GetSection("RabbitMq");
            services.Configure<RabbitMqConfiguration>(serviceClientSettingsConfig);
            RabbitMqConfiguration serviceClientSettings = serviceClientSettingsConfig.Get<RabbitMqConfiguration>();
            if (serviceClientSettings.Enabled)
            {
                services.AddHostedService<SyncItemReceiver>();
            }
            services.AddScoped<ISyncItemSender, SyncItemSender>();
        }
    }
}
