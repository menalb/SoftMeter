using System;
using System.Linq;
using System.Collections.Generic;

using SmartMeter.Core.Models;

namespace SmartMeter.Core
{

    public class Meter
    {
        private readonly INotifier _notifier;
        private readonly MeterInfo _meterInfo;
        private readonly MeterStatus _meterStatus;
        private readonly CustomerInfo _customerInfo;
        private readonly IList<CreditTransaction> _creditTransactions;
        private readonly IList<CreditTransaction> _deltaCreditTransactions;

        private Meter(INotifier notifier)
        {
            _notifier = notifier;
        }

        private Meter(INotifier notifier, MeterInfo meterInfo) : this(notifier)
        {
            _meterInfo = meterInfo;
            _meterStatus = new MeterStatus();
            _creditTransactions = new List<CreditTransaction>();
            _deltaCreditTransactions = new List<CreditTransaction>();
        }

        public Meter(INotifier notifier, MeterInfo meterInfo, CustomerInfo customerInfo) : this(notifier, meterInfo)
        {
            _customerInfo = customerInfo;
        }

        public void SendNotification()
        {
            _notifier.Notify(new SmartMeterNotification(_meterInfo, _customerInfo)
            {
                CreditAmount = _meterStatus.AccountBalance,

                OutstandingDebt = _meterStatus.OutstandingDebt,
                DebtDeducted = _meterStatus.DebtDeducted,
                DebtRecoveryRate = _meterStatus.DebtRecoveryRate,

                TxnAmount = _deltaCreditTransactions.Sum(transaction => transaction.Amount),
                AgentNo = _deltaCreditTransactions.Any() ? _deltaCreditTransactions.Last().AgentNo : string.Empty,
                RetailerCode = _deltaCreditTransactions.Any() ? _deltaCreditTransactions.Last().RetailerCode : string.Empty,
                TxnType = _deltaCreditTransactions.Any() ? _deltaCreditTransactions.Last().TType : 0,
                RequesterId = _deltaCreditTransactions.Any() ? _deltaCreditTransactions.Last().RequesterId : string.Empty,
                PaymentMode = _deltaCreditTransactions.Any() ? _deltaCreditTransactions.Last().PaymentMode : 0,
                SupplyType = 0,

                ReadingTime = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                IssueTime = DateTime.UtcNow,
                IsTxnCancelled = false
            });

            _deltaCreditTransactions.Clear();
        }

        public void TopUp(TopUpRequest request)
        {
            _meterStatus.AccountBalance += request.Amount;
            var transaction = new CreditTransaction
            {
                TDate = DateTime.UtcNow,
                Amount = request.Amount,
                Code = request.Code,
                VendType = request.VendType,
                AgentNo = request.AgentNo,
                RetailerCode = request.RetailerCode,
                RequesterId = request.RequesterId,
                PaymentMode = request.PaymentMode
            };

            _deltaCreditTransactions.Add(transaction);
            _creditTransactions.Add(transaction);
        }

        public void ConsumeEnergy()
        {
            var randomConsume = new Random().Next(1, 50);
            _meterStatus.Reading += randomConsume;
            _meterStatus.AccountBalance -= (randomConsume / 10);
        }
    }
}