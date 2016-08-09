using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

using SmartMeter.Core;
using SmartMeter.Core.Models;

namespace SmartMeter.VirtualMeter
{
    public class Notifier : INotifier
    {
        private readonly string _listenerUrl;

        public Notifier(string listenerUrl)
        {
            _listenerUrl = listenerUrl;
        }

        public void Notify(SmartMeterNotification notification)
        {
            Console.WriteLine($"CreditAmount: {notification.CreditAmount} euro, RetailerCode: {notification.RetailerCode}, ReadingTime: {notification.ReadingTime}, CustomerName: {notification.CustomerName}, TransactionAmount: {notification.TxnAmount}");
            try
            {
                Send(notification);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to send notification {0}", ex);
            }

        }

        private void Send(SmartMeterNotification messages)
        {
            using (var client = GetClient())
            {
                var response = client.PostAsync(_listenerUrl, BuildJson(messages)).Result;
                if (response.IsSuccessStatusCode)
                    Console.WriteLine("[Notification] successfully sent");
                else
                    Console.WriteLine($"[Notification] failed {response.StatusCode.ToString()}");
            }
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private StringContent BuildJson(SmartMeterNotification request)
        {
            var json = JsonConvert.SerializeObject(request);
            return new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        }

    }
}