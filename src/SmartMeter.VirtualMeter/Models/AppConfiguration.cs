namespace SmartMeter.VirtualMeter.Models
{
    internal class AppConfiguration
    {
        public string ListenerUri { get; set; }
        public string MessagesQueueAddress { get; set; }
        public string IncomingMessagesQueue { get; set; }
    }
}