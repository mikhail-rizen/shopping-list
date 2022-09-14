using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ShoppingList.Items.Messaging.Configuration;
using ShoppingList.Items.Service.Model;
using ShoppingList.Items.Service.Service;
using System.Text;

namespace ShoppingList.Items.Messaging.Receive
{
    public class SyncItemReceiver : BackgroundService
    {
        private IModel? channel;
        private IConnection? connection;
        private readonly string? hostname;
        private readonly string? queueName;
        private readonly string? exchangeName;
        private readonly string? username;
        private readonly string? password;
        private readonly IServiceProvider serviceProvider;

        public SyncItemReceiver(IServiceProvider serviceProvider, IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            hostname = rabbitMqOptions.Value.Hostname;
            queueName = rabbitMqOptions.Value.QueueName;
            exchangeName = rabbitMqOptions.Value.ExchangeName;
            username = rabbitMqOptions.Value.UserName;
            password = rabbitMqOptions.Value.Password;
            InitializeRabbitMqListener();
            this.serviceProvider = serviceProvider;
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = hostname,
                UserName = username,
                Password = password,
                DispatchConsumersAsync = true //set false for sync - some RabbitMQ client hack for async consumers (see https://www.rabbitmq.com/dotnet-api-guide.html#consuming-async )
            };

            connection = factory.CreateConnection();
            connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown!;
            channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout, durable: false, autoDelete: false, arguments: null);
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queueName, exchangeName, string.Empty, null);
        }

        #region async implementation

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (ch, ea) => 
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var syncItemModel = JsonConvert.DeserializeObject<SyncItemModel>(content);

                await HandleMessage(syncItemModel, CancellationToken.None);

                channel?.BasicAck(ea.DeliveryTag, false);
            };
            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerCancelled;

            channel.BasicConsume(queueName, false, consumer);

            return Task.CompletedTask;
        }

        private async Task HandleMessage(SyncItemModel syncItemModel, CancellationToken cancellationToken)
        {
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                ISyncItemService syncItemService = scope.ServiceProvider.GetService<ISyncItemService>()!;
                await syncItemService.SyncItem(syncItemModel, CancellationToken.None);
            }
        }

        private Task OnConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            return Task.CompletedTask;
        }

        private Task OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
            return Task.CompletedTask;
        }

        private Task OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
            return Task.CompletedTask;
        }

        private Task OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
            return Task.CompletedTask;
        }

        #endregion

        #region sync implementation
        /*
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var syncItemModel = JsonConvert.DeserializeObject<SyncItemModel>(content);

                HandleMessage(syncItemModel);

                channel!.BasicAck(ea.DeliveryTag, false);
            };
            consumer.Shutdown += OnConsumerShutdown!;
            consumer.Registered += OnConsumerRegistered!;
            consumer.Unregistered += OnConsumerUnregistered!;
            consumer.ConsumerCancelled += OnConsumerCancelled!;

            channel.BasicConsume(queueName, false, consumer);

            return Task.CompletedTask;
        }

        private void HandleMessage(SyncItemModel syncItemModel)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                ISyncItemService syncItemService = scope.ServiceProvider.GetService<ISyncItemService>()!;
                syncItemService.SyncItem(syncItemModel, CancellationToken.None).GetAwaiter().GetResult();
            }
        }

        private void OnConsumerCancelled(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
        }
        */
        #endregion

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
        }

        public override void Dispose()
        {
            channel?.Close();
            connection?.Close();
            base.Dispose();
        }
    }
}