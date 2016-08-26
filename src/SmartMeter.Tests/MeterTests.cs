using System.Collections.Generic;
using System.Linq;
using Xunit;

using SmartMeter.Core;
using SmartMeter.Core.Models;
using System;

namespace SmartMeter.Tests
{
    public class MeterTests
    {
        private readonly Notifier_Fake notifier;
        private Meter _sut;

        public MeterTests()
        {
            notifier = new Notifier_Fake();
        }

        [Fact]
        public void GivenASmartMeter_ItSendsANotificationForTheProvidedMeter()
        {
            var meterInfo = MeterInfoExtensions.Get().WithInitialDataSet();
            var customerInfo = CustomerInfoExtensions.Get().WithInitialDataSet();
            InitializeMeter(meterInfo, customerInfo);

            _sut.SendNotification();

            VerifyThatTheNotificationHasBeenSent(meterInfo);
        }

        [Fact]
        public void GivenASmartMeter_WhenItDoesNotConsumeEnergy_TheNextNotificationContainsTheSameCreditAmountAsTheOneBefore()
        {
            decimal initialAmount = 50;
            var meterInfo = MeterInfoExtensions.Get().WithInitialDataSet();
            var customerInfo = CustomerInfoExtensions.Get().WithInitialDataSet();
            InitializeMeter(meterInfo, customerInfo, initialAmount);

            _sut.SendNotification();

            VerifyThatTheNotificationWithTheSameCreditAmountHasBeenSent(meterInfo, initialAmount);
        }

        [Fact]
        public void GivenASmartMeter_WhenItConsumeEnergy_TheNextNotificationContainsALowerCreditAmount()
        {
            decimal initialAmount = 50;
            var meterInfo = MeterInfoExtensions.Get().WithInitialDataSet();
            var customerInfo = CustomerInfoExtensions.Get().WithInitialDataSet();
            InitializeMeter(meterInfo, customerInfo, initialAmount);

            _sut.ConsumeEnergy();
            _sut.SendNotification();

            VerifyThatTheNotificationWithALowerCreditAmountHasBeenSent(meterInfo, initialAmount);
        }

        [Fact]
        public void GivenASmartMeter_WhenItDoesNotConsumeEnergyAndTopUp_TheNextNotificationContainsAHigherCreditAmount()
        {
            decimal initialAmount = 50;
            var meterInfo = MeterInfoExtensions.Get().WithInitialDataSet();
            var customerInfo = CustomerInfoExtensions.Get().WithInitialDataSet();
            InitializeMeter(meterInfo, customerInfo, initialAmount);
            var topUp = TopUpRequestExtensions.Get().WithAmount(20);

            _sut.TopUp(topUp);
            _sut.SendNotification();

            VerifyThatTheNotificationWithAnHigherCreditAmountHasBeenSent(meterInfo, initialAmount, 20);
        }

        [Fact]
        public void GivenASmartMeter_WhenItSendANotification_TheNotificationContainsUserInfo()
        {
            var meterInfo = MeterInfoExtensions.Get().WithInitialDataSet();
            var customerInfo = CustomerInfoExtensions.Get().WithInitialDataSet();
            var topUp = TopUpRequestExtensions.Get().WithAmount(50);
            InitializeMeter(meterInfo, customerInfo, 50);

            _sut.TopUp(topUp);
            _sut.SendNotification();

            VerifyThatTheNotificationContainsUserInfo(meterInfo, customerInfo);
        }

        [Fact]
        public void GivenASmartMeter_WhenNoTransactionHasBeenPerformed_TheTransactionAmountInTheNotificationIsZero()
        {
            var meterInfo = MeterInfoExtensions.Get().WithInitialDataSet();
            var customerInfo = CustomerInfoExtensions.Get().WithInitialDataSet();
            InitializeMeter(meterInfo, customerInfo);

            _sut.SendNotification();

            VerifyThatTheTransactionAmountInTheNotificationIsZero(meterInfo);
        }

        [Fact]
        public void GivenASmartMeter_WhenATransactionHasBeenPerformed_TheTransactionAmountInTheNotificationIsEqualsToTheTopUpAmount()
        {
            var meterInfo = MeterInfoExtensions.Get().WithInitialDataSet();
            var customerInfo = CustomerInfoExtensions.Get().WithInitialDataSet();
            var topUp = TopUpRequestExtensions.Get().WithAmount(20);
            InitializeMeter(meterInfo, customerInfo);

            _sut.TopUp(topUp);
            _sut.SendNotification();

            VerifyThatTheTransactionAmountInTheNotificationIsEqualsToTheTopUpAMount(meterInfo, topUp.Amount);
        }

        [Fact]
        public void GivenASmartMeter_WhenSomeTransactionHaveBeenPerformed_TheTransactionAmountInTheNotificationIsEqualsToTheSumOfTheTopUpAmount()
        {
            var meterInfo = MeterInfoExtensions.Get().WithInitialDataSet();
            var customerInfo = CustomerInfoExtensions.Get().WithInitialDataSet();
            var topUp1 = TopUpRequestExtensions.Get().WithAmount(20);
            var topUp2 = TopUpRequestExtensions.Get().WithAmount(30);
            InitializeMeter(meterInfo, customerInfo);

            _sut.TopUp(topUp1);
            _sut.TopUp(topUp2);
            _sut.SendNotification();

            VerifyThatTheTransactionAmountInTheNotificationIsEqualsToTheTopUpAMount(meterInfo, topUp1.Amount + topUp2.Amount);
        }

        [Fact]
        public void GivenASmartMeter_WhenANotificationHasBeenSent_TheNextNotificationContainsTheDeltaFromTheLastNotification()
        {
            var meterInfo = MeterInfoExtensions.Get().WithInitialDataSet();
            var customerInfo = CustomerInfoExtensions.Get().WithInitialDataSet();
            var topUpPre = TopUpRequestExtensions.Get().WithAmount(20);
            var topUpPost = TopUpRequestExtensions.Get().WithAmount(30);
            InitializeMeter(meterInfo, customerInfo);

            _sut.TopUp(topUpPre);
            _sut.SendNotification();

            VerifyThatTheTransactionAmountInTheNotificationIsEqualsToTheTopUpAMount(meterInfo, topUpPre.Amount);

            _sut.TopUp(topUpPost);
            _sut.SendNotification();

            VerifyThatTheTransactionAmountInTheNotificationIsEqualsToTheTopUpAMount(meterInfo, topUpPost.Amount);
        }

        private void InitializeMeter(MeterInfo meterInfo, CustomerInfo customerInfo)
        {
            _sut = new Meter(notifier, meterInfo, customerInfo);
        }

        private void InitializeMeter(MeterInfo meterInfo, CustomerInfo customerInfo, decimal initialAmount)
        {
            InitializeMeter(meterInfo, customerInfo);
            _sut.TopUp(TopUpRequestExtensions.Get().WithAmount(initialAmount));
        }

        private SmartMeterNotification VerifyThatTheNotificationHasBeenSent(MeterInfo meterInfo)
        {
            var notificationsSent = notifier.NotificationsSent();

            return notificationsSent.Where(notificationSent =>
                notificationSent.SupplierCode == meterInfo.SupplierCode &&
                notificationSent.MeterNo == meterInfo.MeterNo &&
                notificationSent.MfgMeterNo == meterInfo.MfgMeterNo &&
                notificationSent.TerminalId == meterInfo.TerminalId &&
                notificationSent.ServicePoint == meterInfo.ServicePoint &&
                notificationSent.MeterId == meterInfo.MeterId
            ).Last();
        }

        private void VerifyThatTheNotificationWithALowerCreditAmountHasBeenSent(MeterInfo meterInfo, decimal initialCreditAmount)
        {
            var notification = VerifyThatTheNotificationHasBeenSent(meterInfo);
            Assert.True(notification.CreditAmount < initialCreditAmount);
        }

        private void VerifyThatTheNotificationWithTheSameCreditAmountHasBeenSent(MeterInfo meterInfo, decimal initialCreditAmount)
        {
            var notification = VerifyThatTheNotificationHasBeenSent(meterInfo);
            Assert.Equal(notification.CreditAmount, initialCreditAmount);
        }

        private void VerifyThatTheNotificationWithAnHigherCreditAmountHasBeenSent(MeterInfo meterInfo, decimal initialCreditAmount, decimal topUpAmount)
        {
            var notification = VerifyThatTheNotificationHasBeenSent(meterInfo);
            Assert.Equal(notification.CreditAmount, initialCreditAmount + topUpAmount);
        }

        private void VerifyThatTheTransactionAmountInTheNotificationIsZero(MeterInfo meterInfo)
        {
            var notification = VerifyThatTheNotificationHasBeenSent(meterInfo);
            Assert.Equal(notification.TxnAmount, 0);
        }

        public void VerifyThatTheTransactionAmountInTheNotificationIsEqualsToTheTopUpAMount(MeterInfo meterInfo, decimal topUpAmount)
        {
            var notification = VerifyThatTheNotificationHasBeenSent(meterInfo);
            Assert.Equal(notification.TxnAmount, topUpAmount);
        }

        private void VerifyThatTheNotificationContainsUserInfo(MeterInfo meterInfo, CustomerInfo userInfo)
        {
            var notification = VerifyThatTheNotificationHasBeenSent(meterInfo);
            Assert.Equal(notification.CustomerName, userInfo.CustomerName);
            Assert.Equal(notification.CardId, userInfo.CardId);
            Assert.Equal(notification.VatRate, userInfo.VatRate);
            Assert.Equal(notification.VatOnEnergy, userInfo.VatOnEnergy);
            Assert.Equal(notification.VatOnDebt, userInfo.VatOnDebt);
        }
    }

    internal static class MeterInfoExtensions
    {
        public static MeterInfo Get()
        {
            return new MeterInfo(1, "TEST");
        }

        public static MeterInfo WithInitialDataSet(this MeterInfo meterInfo)
        {
            meterInfo.SupplierCode = "PPP";
            meterInfo.MeterNo = "SMU91033";
            meterInfo.MfgMeterNo = "0000000001148533";
            meterInfo.VendCode = "26443772002727999783";
            return meterInfo;
        }
    }

    internal static class CustomerInfoExtensions
    {
        public static CustomerInfo Get()
        {
            return new CustomerInfo();
        }

        public static CustomerInfo WithInitialDataSet(this CustomerInfo customer)
        {
            customer.CardId = "9826176066400000038";
            customer.CustomerName = "Prepay Power";
            customer.VatOnDebt = 0;
            customer.VatOnEnergy = 59;
            customer.VatRate = 13.5m;

            return customer;
        }
    }

    internal static class TopUpRequestExtensions
    {
        public static TopUpRequest Get()
        {
            return new TopUpRequest { MeterId = 403030, Amount = 50, RequesterId = "12345", AgentNo = "FREE", Code = new Guid().ToString(), PaymentMode = 0, RetailerCode = "PAYPOINT", VendType = 0 };
        }

        public static TopUpRequest WithAmount(this TopUpRequest topUpRequest, decimal amount)
        {
            topUpRequest.Amount = amount;
            return topUpRequest;
        }
    }

    internal class Notifier_Fake : INotifier
    {
        private readonly IList<SmartMeterNotification> _notificationsSent;

        public Notifier_Fake()
        {
            _notificationsSent = new List<SmartMeterNotification>();
        }

        public IList<SmartMeterNotification> NotificationsSent()
        {
            return _notificationsSent;
        }

        public void Notify(SmartMeterNotification notification)
        {
            _notificationsSent.Add(notification);
        }
    }
}