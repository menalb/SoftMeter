namespace SmartMeter.Core.Models
{
    public class CustomerInfo
    {
        public string CardId { get; set; }
        public string CustomerName { get; set; }
        public decimal VatRate { get; set; }
        public decimal VatOnDebt { get; set; }
        public decimal VatOnEnergy { get; set; }
    }
}