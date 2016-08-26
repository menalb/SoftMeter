namespace SoftMeter.UIConsole.Models.Messages
{
    public class TopUpRequest
    {
        public decimal Amount { get; set; }
        public string RequesterId { get; set; }
        public int WseId { get; set; }
    }
}