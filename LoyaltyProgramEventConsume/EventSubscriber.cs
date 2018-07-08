using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
using static System.Console;

namespace LoyaltyProgramEventConsume
{
    public class EventSubscriber
    {
        private readonly string loyaltyProgramHost;
        private long start = 1, chunkSize = 0;
        private readonly Timer timer;

        public EventSubscriber(string loyaltyProgramHost)
        {
            WriteLine("created");
            this.loyaltyProgramHost = loyaltyProgramHost;
            this.timer = new Timer(5 * 1000); // устанавливаем таймер на 10 секунд
            this.timer.AutoReset = false;
            this.timer.Elapsed += (_, __) => SubscriptionCycleCallback().Wait(); // вызываем каждый раз, когда проходит заданное в таймере время
        }

        private async Task SubscriptionCycleCallback()
        {
            var response = await ReadEvents();
            if (response.StatusCode == HttpStatusCode.OK)
                HandleEvents(await response.Content.ReadAsStringAsync());
            this.timer.Start();
        }

        private async Task<HttpResponseMessage> ReadEvents()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri($"http://{this.loyaltyProgramHost}");
                var response = await httpClient.GetAsync($"/events/?firstEventSequenceNumber={this.start}&lastEventSequenceNumber={this.start + this.chunkSize}");
                WriteLine("Get Events: " + response?.RequestMessage.RequestUri);
                await PrettyPrintResponse(response);
                return response;
            }
        }

        private void HandleEvents(string content)
        {
            WriteLine("Handling events");
            var events = JsonConvert.DeserializeObject<IEnumerable<Event>>(content);
            WriteLine(events);
            WriteLine(events.Count());
            foreach (var ev in events)
            {
                WriteLine(ev.Content);
                dynamic eventData = ev.Content;
              //  WriteLine("product name from data: " + (string)eventData.item.productName); //error
                this.start = Math.Max(this.start, ev.SequenceNumber + 1);
            }
        }

        public void Start()
        {
            this.timer.Start();
        }

        public void Stop()
        {
            this.timer.Stop();
        }

        private static async Task PrettyPrintResponse(HttpResponseMessage response)
        {
            WriteLine("Status code: " + response?.StatusCode.ToString() ?? "command failed");
            WriteLine("Headers: " + response?.Headers.Aggregate("", (acc, h) => acc + "\n\t" + h.Key + ": " + h.Value) ?? "");
            WriteLine("Body: " + await response?.Content.ReadAsStringAsync() ?? "");
        }
    }
}
