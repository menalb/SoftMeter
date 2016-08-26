namespace SmartMeter.Core.Models
{
    public class TopUpRequest
    {
        public int MeterId { get; set; }
        public decimal Amount { get; set; }
        public string RequesterId { get; set; }
        public string Code { get; set; }
        public int VendType { get; set; }
        public string AgentNo { get; set; }
        public string RetailerCode { get; set; }
        public int PaymentMode { get; set; }
    }
}