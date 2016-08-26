using System;

namespace SoftMeter.UIConsole.Models
{
    public class SoftMeterNotification
    {
        public DateTime IssueTime { get; set; }
        public string CardId { get; set; }
        public string SupplierCode { get; set; }
        public string RetailerCode { get; set; }
        public string TerminalId { get; set; }
        public string AgentNo { get; set; }
        public int TxnType { get; set; }
        public string RequesterId { get; set; }
        public int MeterId { get; set; }
        public int SupplyType { get; set; }
        public string ServicePoint { get; set; }
        public string CustomerName { get; set; }
        public string MeterNo { get; set; }
        public string MfgMeterNo { get; set; }
        public string VendCode { get; set; }
        public decimal TxnAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal DebtRecoveryRate { get; set; }
        public int DebtDeducted { get; set; }
        public int OutstandingDebt { get; set; }
        public decimal VatRate { get; set; }
        public decimal VatOnEnergy { get; set; }
        public decimal VatOnDebt { get; set; }
        public int PaymentMode { get; set; }
        public bool IsTxnCancelled { get; set; }
        public DateTimeOffset ReadingTime { get; set; }
    }
}