using System;

using SmartMeter.Core;
using SmartMeter.Core.Models;
using System.Xml;

namespace XmlNotifier
{
    public class XmlNotifier : INotifier
    {
        public void Notify(SmartMeterNotification notification)
        {
            Console.WriteLine($"CreditAmount: {notification.CreditAmount} euro, RetailerCode: {notification.RetailerCode}, ReadingTime: {notification.ReadingTime}, CustomerName: {notification.CustomerName}, TransactionAmount: {notification.TxnAmount}");

            var settings = new XmlWriterSettings();
            settings.Indent = true;
            //var serializer = XmlWriter.Create("test.xml", settings);
        }
    }
}