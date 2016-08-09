using SmartMeter.Core.Models;

namespace SmartMeter.Core
{
    public interface INotifier
    {
        void Notify(SmartMeterNotification notification);
    }
}