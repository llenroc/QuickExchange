using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CryptoTools.Core.Interfaces;
using Newtonsoft.Json;

namespace CryptoTools.Core.NotificationManagers
{
    public class SlackNotificationManager : INotificationManager
    {
        public string TeamId { get; set; }
        public string BotId { get; set; }
        public string Token { get; set; }
        
        public async Task<bool> SendNotification(string message)
        {
            try
            {
                var url = $"https://hooks.slack.com/services/{TeamId}/{BotId}/{Token}";
                var payload = new Payload() {text = message};
                var payloadJson = JsonConvert.SerializeObject(payload);

                payloadJson = payloadJson.Replace(@"\\n", @"\n");

                var httpClient = new HttpClient();
                var response = await httpClient.PostAsync(url, new StringContent(payloadJson, Encoding.UTF8, "application/json"));

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SendTemplatedNotification(string template, params object[] parameters)
        {
            var finalMessage = string.Format(template, parameters);
           return await SendNotification(finalMessage);
        }

        public class Payload
        {
            public string text { get; set; }
        }
    }
}
