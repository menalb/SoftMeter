using Newtonsoft.Json;
using RabbitMQ.Client;
using SoftMeter.UIConsole.Models;
using SoftMeter.UIConsole.Models.Messages;
using System;
using System.Text;

namespace SoftMeter.UIConsole.Services
{
    public interface IMeterService
    {
        void StartNew(StartNewMeterRequest newMeter);
    }

    public class MeterService : IMeterService
    {
        private readonly string _hostname;

        public MeterService(string hostname)
        {
            _hostname = hostname;
        }

        public void StartNew(StartNewMeterRequest newMeter)
        {
            var factory = new ConnectionFactory { HostName = _hostname };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: nameof(StartNewMeterRequest), type: "fanout");

                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(newMeter));

                    channel.BasicPublish(exchange: nameof(StartNewMeterRequest),
                                 routingKey: "",
                                 basicProperties: null,
                                 body: body);
                }
            }
        }
    }
}