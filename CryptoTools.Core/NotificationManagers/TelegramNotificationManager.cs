using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CryptoTools.Core.Interfaces;
using Newtonsoft.Json;

namespace CryptoTools.Core.NotificationManagers
{
    public class TelegramNotificationManager : INotificationManager
    {
        public string BotToken { get; set; }
        public string ChatId { get; set; }

        public async Task<bool> SendNotification(string message)
        {
            try
            {
                var url = $"https://api.telegram.org/bot" + BotToken + "/sendMessage";

                using (var httpClient = new HttpClient())
                {
                    var dictionary = new Dictionary<string, string>
                    {
                        {"chat_id", ChatId},
                        {"parse_mode", "Markdown"},
                        {"text", message}
                    };

                    var json = JsonConvert.SerializeObject(dictionary);
                    
                    json = json.Replace(@"\\n", @"\n");

                    var requestData = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(string.Format(url), requestData);

                    return response.IsSuccessStatusCode;
                }
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
    }
}
