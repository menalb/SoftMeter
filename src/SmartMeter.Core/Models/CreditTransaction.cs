using System;

namespace SmartMeter.Core.Models
{
    public class CreditTransaction
    {
        public string Code { get; set; }
        public DateTime TDate { get; set; }
        public int VendType { get; set; }
        public int TType { get; set; }
        public decimal Amount { get; set; }
        public string AgentNo { get; set; }
        public string RetailerCode { get; set; }
        public string RequesterId { get; set; }
        public int PaymentMode { get; set; }
    }
}