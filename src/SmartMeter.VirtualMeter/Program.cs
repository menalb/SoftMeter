using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

using SmartMeter.Core.Models;
using SmartMeter.VirtualMeter.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using System.Threading;

namespace SmartMeter.VirtualMeter
{
    public class Program
    {
        private static IConfiguration Configuration { get; set; }

        public static void Main(string[] args)
        {
            var appConfiguration = BuildConfiguration();

            var newMeterListenerTask = new Task(() => ServerMeterListener.ListenStartNewMeter(
                appConfiguration.MessagesQueueAddress,
                s =>
                {
                    var task = StartTask(appConfiguration, s.WseId);
                    task.Start();
                }));

            newMeterListenerTask.Start();

            Task.WaitAll(newMeterListenerTask);

            Console.WriteLine("Finished");

            Console.ReadKey();
        }

        private static Task StartTask(AppConfiguration appConfiguration, int wseId)
        {
            return new Task(() => new SingleMeter(appConfiguration, new MeterInfo(wseId, "FIRST")
            {
                SupplierCode = "PPP",
                MeterNo = "SMU91033",
                MfgMeterNo = "0000000001148533"
            },
            new CustomerInfo
            {
                CardId = "9826176066400000038",
                CustomerName = "Prepay Power " + wseId.ToString(),
                VatOnDebt = 0,
                VatOnEnergy = 59,
                VatRate = 13.5m
            },
            new TopUpRequest { WseId = 403030, Amount = 10, RequesterId = "111111", AgentNo = "FREE", Code = new Guid().ToString(), PaymentMode = 0, RetailerCode = "PAYPOINT", VendType = 0 }
            ).TurnOn());
        }

        private static Task StartTask1(AppConfiguration appConfiguration)
        {
            return new Task(() => new SingleMeter(appConfiguration, new MeterInfo(403030, "FIRST")
            {
                SupplierCode = "PPP",
                MeterNo = "SMU91033",
                MfgMeterNo = "0000000001148533"
            },
            new CustomerInfo
            {
                CardId = "9826176066400000038",
                CustomerName = "Prepay Power 1",
                VatOnDebt = 0,
                VatOnEnergy = 59,
                VatRate = 13.5m
            },
            new TopUpRequest { WseId = 403030, Amount = 10, RequesterId = "111111", AgentNo = "FREE", Code = new Guid().ToString(), PaymentMode = 0, RetailerCode = "PAYPOINT", VendType = 0 }
            ).TurnOn());
        }

        private static Task StartTask2(AppConfiguration appConfiguration)
        {
            return new Task(() => new SingleMeter(appConfiguration, new MeterInfo(403032, "SECOND")
            {
                SupplierCode = "PPP",
                MeterNo = "SMU91033",
                MfgMeterNo = "0000000001148533"
            },
                        new CustomerInfo
                        {
                            CardId = "9826176066400000039",
                            CustomerName = "Prepay Power 2",
                            VatOnDebt = 0,
                            VatOnEnergy = 59,
                            VatRate = 13.5m
                        },
                        new TopUpRequest { WseId = 403030, Amount = 150, RequesterId = "111111", AgentNo = "FREE", Code = new Guid().ToString(), PaymentMode = 0, RetailerCode = "PAYPOINT", VendType = 0 }
                       ).TurnOn());
        }

        private static AppConfiguration BuildConfiguration()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            return new AppConfiguration
            {
                ListenerUri = Configuration.GetValue<string>("ListenerUri"),
                MessagesQueueAddress = Configuration.GetValue<string>("MqssaggeQueueAddress"),
                IncomingMessagesQueue = Configuration.GetValue<string>("IncomingMessagesQueue")
            };

        }
    }

    public class ServerMeterListener
    {
        public static void ListenStartNewMeter(string hostname, Action<StartNewMeterRequest> onMessageReceived)
        {
            var factory = new ConnectionFactory { HostName = hostname };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: nameof(StartNewMeterRequest), type: "fanout");

                    var queueName = channel.QueueDeclare().QueueName;

                    channel.QueueBind(queue: queueName, exchange: nameof(StartNewMeterRequest), routingKey: "");

                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);

                        var startNewMeterRequest = JsonConvert.DeserializeObject<StartNewMeterRequest>(message);

                        Console.WriteLine($"Start new Meter request: {message}");

                        onMessageReceived?.Invoke(startNewMeterRequest);

                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    };

                    channel.BasicConsume(queue: queueName, noAck: false, consumer: consumer);
                    while (true) { Thread.Sleep(5000); };
                }
            }
        }
    }

    public class StartNewMeterRequest
    {
        public int WseId { get; set; }
    }

}