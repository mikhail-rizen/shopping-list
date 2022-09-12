using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using ShoppingList.Items.Entities;
using ShoppingList.Items.Messaging.Configuration;
using System.Text;

namespace ShoppingList.Items.Messaging.Send
{
    public class SyncItemSender : ISyncItemSender
    {
        private readonly string? hostname;
        private readonly string? password;
        private readonly string? queueName;
        private readonly string? username;
        private IConnection? connection;

        public SyncItemSender(IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            queueName = rabbitMqOptions.Value.QueueName;
            hostname = rabbitMqOptions.Value.Hostname;
            username = rabbitMqOptions.Value.UserName;
            password = rabbitMqOptions.Value.Password;

            CreateConnection();
        }

        public async Task SendSyncItem(Item item, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await Task.Yield();

            if (ConnectionExists())
            {
                using (var channel = connection!.CreateModel())
                {
                    channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var json = JsonConvert.SerializeObject(item);
                    var body = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                }
            }
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = hostname,
                    UserName = username,
                    Password = password
                };
                connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not create connection: {ex.Message}");
            }
        }

        private bool ConnectionExists()
        {
            if (connection != null)
            {
                return true;
            }

            CreateConnection();

            return connection != null;
        }
    }
}