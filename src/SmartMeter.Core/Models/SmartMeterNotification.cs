using System;

namespace SmartMeter.Core.Models
{
    public class SmartMeterNotification
    {
        public DateTime IssueTime { get; set; }
        public string CardId { get; set; }
        public string SupplierCode { get; set; }
        public string RetailerCode { get; set; }
        public string TerminalId { get; set; }
        public string AgentNo { get; set; }
        public int TxnType { get; set; }
        public string RequesterId { get; set; }
        public int WseId { get; set; }
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

        public SmartMeterNotification() { }

        public SmartMeterNotification(MeterInfo meterInfo, CustomerInfo customerInfo)
        {
            SetMeterInfo(meterInfo);
            SetCustomerInfo(customerInfo);
        }

        private void SetMeterInfo(MeterInfo meterInfo)
        {
            MeterNo = meterInfo.MeterNo;
            MfgMeterNo = meterInfo.MfgMeterNo;
            ServicePoint = meterInfo.ServicePoint;
            SupplierCode = meterInfo.SupplierCode;
            TerminalId = meterInfo.TerminalId;
            WseId = meterInfo.WseId;
            VendCode = meterInfo.VendCode;
        }

        private void SetCustomerInfo(CustomerInfo customerInfo)
        {
            CardId = customerInfo.CardId;
            CustomerName = customerInfo.CustomerName;
            VatOnDebt = customerInfo.VatOnDebt;
            VatOnEnergy = customerInfo.VatOnEnergy;
            VatRate = customerInfo.VatRate;
        }

        public bool Equals(SmartMeterNotification notification)
        {
            return notification.IssueTime == IssueTime &&
               notification.CardId == CardId &&
               notification.SupplierCode == SupplierCode &&
               notification.RetailerCode == RetailerCode &&
               notification.TerminalId == TerminalId &&
               notification.AgentNo == AgentNo &&
               notification.TxnType == TxnType &&
               notification.RequesterId == RequesterId &&
               notification.WseId == WseId &&
               notification.SupplyType == SupplyType &&
               notification.ServicePoint == ServicePoint &&
               notification.CustomerName == CustomerName &&
               notification.MeterNo == MeterNo &&
               notification.MfgMeterNo == MfgMeterNo &&
               notification.VendCode == VendCode &&
               notification.TxnAmount == TxnAmount &&
               notification.CreditAmount == CreditAmount &&
               notification.DebtRecoveryRate == DebtRecoveryRate &&
               notification.DebtDeducted == DebtDeducted &&
               notification.OutstandingDebt == OutstandingDebt &&
               notification.VatRate == VatRate &&
               notification.VatOnEnergy == VatOnEnergy &&
               notification.VatOnDebt == VatOnDebt &&
               notification.PaymentMode == PaymentMode &&
               notification.IsTxnCancelled == IsTxnCancelled &&
               notification.ReadingTime == ReadingTime
               ;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            var notification = (SmartMeterNotification)obj;

            return notification.Equals(obj);
        }

        public override int GetHashCode()
        {
            var hashCode = 13;
            hashCode = (hashCode * 7) + IssueTime.GetHashCode();
            hashCode = (hashCode * 7) + CardId.GetHashCode();
            hashCode = (hashCode * 7) + SupplierCode.GetHashCode();
            hashCode = (hashCode * 7) + RetailerCode.GetHashCode();
            hashCode = (hashCode * 7) + TerminalId.GetHashCode();
            hashCode = (hashCode * 7) + AgentNo.GetHashCode();
            hashCode = (hashCode * 7) + TxnType.GetHashCode();
            hashCode = (hashCode * 7) + RequesterId.GetHashCode();
            hashCode = (hashCode * 7) + WseId.GetHashCode();
            hashCode = (hashCode * 7) + SupplyType.GetHashCode();
            hashCode = (hashCode * 7) + ServicePoint.GetHashCode();
            hashCode = (hashCode * 7) + CustomerName.GetHashCode();
            hashCode = (hashCode * 7) + MeterNo.GetHashCode();
            hashCode = (hashCode * 7) + MfgMeterNo.GetHashCode();
            hashCode = (hashCode * 7) + VendCode.GetHashCode();
            hashCode = (hashCode * 7) + TxnAmount.GetHashCode();
            hashCode = (hashCode * 7) + CreditAmount.GetHashCode();
            hashCode = (hashCode * 7) + DebtRecoveryRate.GetHashCode();
            hashCode = (hashCode * 7) + DebtDeducted.GetHashCode();
            hashCode = (hashCode * 7) + OutstandingDebt.GetHashCode();
            hashCode = (hashCode * 7) + VatRate.GetHashCode();
            hashCode = (hashCode * 7) + VatOnEnergy.GetHashCode();
            hashCode = (hashCode * 7) + VatOnDebt.GetHashCode();
            hashCode = (hashCode * 7) + PaymentMode.GetHashCode();
            hashCode = (hashCode * 7) + IsTxnCancelled.GetHashCode();

            return hashCode;
        }
    }
}