using System.Text;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

var config = new ConfigurationBuilder()
.AddJsonFile($"appsettings.development.json")
.Build();

var factory = new ConnectionFactory
{
  HostName = "localhost",
  Port = 5672,
  UserName = config.GetSection("RabbitMQ").GetSection("Username").Value,
  Password = config.GetSection("RabbitMQ").GetSection("Password").Value
};

using var conn = factory.CreateConnection();
using var chan = conn.CreateModel();

var queueName = "hello";

chan.QueueDeclare(
  queue: queueName,
  durable: false,
  exclusive: false,
  autoDelete: false,
  arguments: null
);

var message = "Hello World!";
var body = Encoding.UTF8.GetBytes(message);

chan.BasicPublish(
  exchange: string.Empty,
  routingKey: queueName,
  basicProperties: null,
  body: body
);

Console.WriteLine($" [x] Sent {message}");
Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();