using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ShoppingList.Items.Service.Service;
using Microsoft.Extensions.Options;
using ShoppingList.Items.Service.Model;
using Newtonsoft.Json;
using System.Text;
using ShoppingList.Items.Messaging.Configuration;

namespace ShoppingList.Items.Messaging.Receive
{
    public class SyncItemReceiver : BackgroundService
    {
        private IModel? channel;
        private IConnection? connection;
        private readonly ISyncItemService syncItemService;
        private readonly string? hostname;
        private readonly string? queueName;
        private readonly string? username;
        private readonly string? password;

        public SyncItemReceiver(ISyncItemService syncItemService, IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            hostname = rabbitMqOptions.Value.Hostname;
            queueName = rabbitMqOptions.Value.QueueName;
            username = rabbitMqOptions.Value.UserName;
            password = rabbitMqOptions.Value.Password;
            this.syncItemService = syncItemService;
            InitializeRabbitMqListener();
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = hostname,
                UserName = username,
                Password = password
            };

            connection = factory.CreateConnection();
            connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown!;
            channel = connection.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (ch, ea) => 
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var updateCustomerFullNameModel = JsonConvert.DeserializeObject<SyncItemModel>(content);

                await HandleMessage(updateCustomerFullNameModel, CancellationToken.None);

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
            await syncItemService.SyncItem(syncItemModel, cancellationToken);
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