using System;
using System.Threading;
using System.Threading.Tasks;

using SmartMeter.Core;
using SmartMeter.Core.Models;
using SmartMeter.VirtualMeter.Models;

namespace SmartMeter.VirtualMeter
{
    internal class SingleMeter
    {
        private readonly string _messagesQueueAddress;
        private readonly string _incomingMessagesQueue;
        private readonly CustomerInfo _customerInfo;
        private readonly MeterInfo _meterInfo;
        private readonly Meter _meter;

        private Timer consumeTimer;
        private Timer sendNotificationTimer;

        public SingleMeter(AppConfiguration appConfiguration, MeterInfo meterInfo, CustomerInfo customerInfo)
        {
            _meterInfo = meterInfo;
            _messagesQueueAddress = appConfiguration.MessagesQueueAddress;
            _incomingMessagesQueue = appConfiguration.IncomingMessagesQueue;
            _customerInfo = customerInfo;

            _meter = new Meter(new Notifier(appConfiguration.ListenerUri), meterInfo, _customerInfo);
        }

        public SingleMeter(AppConfiguration appConfiguration, MeterInfo meterInfo, CustomerInfo customerInfo, TopUpRequest initialTopUp) :
            this(appConfiguration, meterInfo, customerInfo)
        {
            _meter.TopUp(initialTopUp);
        }

        public void TurnOn()
        {
            StartListener(_messagesQueueAddress, _incomingMessagesQueue,
                topUpRequest =>
                {
                    if (_meterInfo.MeterId == topUpRequest.MeterId)
                        _meter.TopUp(topUpRequest);
                });

            StartConsumeTimer(_meter, 10000);
            StartSendNotificationsTimer(_meter, 5000);
            while (true) { Thread.Sleep(5000); };
        }

        private void StartListener(string address, string queue, Action<TopUpRequest> performTopUp)
        {
            Task.Run(() => MeterListener.Listen(address, queue, performTopUp));
        }

        private void StartConsumeTimer(Meter meter, int intervall)
        {
            consumeTimer = new Timer(
                    status => meter.ConsumeEnergy(),
                    new AutoResetEvent(false),
                    0,
                    intervall
                    );
        }

        private void StartSendNotificationsTimer(Meter meter, int intervall)
        {
            sendNotificationTimer = new Timer(
                    status => meter.SendNotification(),
                    new AutoResetEvent(false),
                    0,
                    intervall
                    );
        }
    }
}