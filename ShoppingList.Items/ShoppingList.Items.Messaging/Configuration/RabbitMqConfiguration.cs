namespace ShoppingList.Items.Messaging.Configuration
{
    public class RabbitMqConfiguration
    {
        public string? Hostname { get; set; }

        public string? QueueName { get; set; }

        public string? ExchangeName { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }

        public bool Enabled { get; set; }
    }
}
