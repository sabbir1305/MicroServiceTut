
using Microsoft.Extensions.Configuration;
using PaltformServiceAPI.Dtos;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaltformServiceAPI.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        private readonly string _exchangeType="trigger";

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: _exchangeType, type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutDown;

                Console.WriteLine("Connected with messagebus");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"RabbitMQ failed to connect: {ex.Message}");
            }
        }

        private void RabbitMQ_ConnectionShutDown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("RabbitMQ shutting down");
        }

        public void PublishNewPatform(PlatformPublishDto platform)
        {
            var message = JsonSerializer.Serialize(platform);

            if (_connection.IsOpen)
            {
                Console.WriteLine("-->Connection available --");

                SendMessage(message);
            }
            else
            {
                Console.WriteLine("-->Connection not available --");

            }
        }
   
        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: _exchangeType, routingKey: "", basicProperties: null, body: body);

            Console.WriteLine("--> Message sent");
        }

        public void Dispose()
        {
            Console.WriteLine("Messagebus disposed");

            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    
    }
}
