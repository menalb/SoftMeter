using System.Threading.Tasks;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;
using SoftMeter.UIConsole.Models.Messages;

namespace SoftMeter.UIConsole.Services
{
    public class TopUpSender : ITopUpSender
    {
        private readonly string _hostname;
        private readonly string _queueName;

        public TopUpSender(string hotname, string queueName)
        {
            _hostname = hotname;
            _queueName = queueName;
        }

        public Task TopUpAsync(TopUpRequest request)
        {
            return Task.Run(() => TopUp(request));
        }

        public void TopUp(TopUpRequest request)
        {
            var factory = new ConnectionFactory { HostName = _hostname };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));

                    var channelProperties = channel.CreateBasicProperties();
                    channelProperties.Persistent = true;

                    channel.BasicPublish(exchange: "", routingKey: "SoftMeter", basicProperties: channelProperties, body: body);
                }
            }
        }
    }
}