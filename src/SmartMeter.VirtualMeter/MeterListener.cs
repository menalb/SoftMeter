using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SmartMeter.Core.Models;
using System;
using System.Text;
using System.Threading;

namespace SmartMeter.VirtualMeter
{
    public class MeterListener
    {
        public static void Listen(string hostname, string queueName, Action<TopUpRequest> onMessageReceived)
        {
            var factory = new ConnectionFactory { HostName = hostname };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);

                        var topUpRequest = JsonConvert.DeserializeObject<TopUpRequest>(message);

                        Console.WriteLine($"top up request message: {message}");

                        onMessageReceived?.Invoke(topUpRequest);

                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    };

                    channel.BasicConsume(queue: queueName, noAck: false, consumer: consumer);
                    while (true) { Thread.Sleep(5000); };
                }
            }
        }
    }
}