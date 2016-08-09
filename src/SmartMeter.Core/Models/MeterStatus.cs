namespace SmartMeter.Core.Models
{
    public class MeterStatus
    {
        public decimal AccountBalance { get; set; }
        public decimal Reading { get; set; }

        public int OutstandingDebt { get; set; }
        public int DebtDeducted { get; set; }
        public int DebtRecoveryRate { get; set; }
    }

    public class MeterInfo
    {
        public MeterInfo(int wseId, string terminalId)
        {
            WseId = wseId;
            TerminalId = terminalId;
        }

        public string SupplierCode { get; set; }        
        public string TerminalId { get; }
        public int WseId { get; }        
        public string ServicePoint { get; }
        public string MeterNo { get; set; }
        public string MfgMeterNo { get; set; }
        public string VendCode { get; set; }
    }
}