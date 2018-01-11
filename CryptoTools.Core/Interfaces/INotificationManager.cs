using System.Threading.Tasks;

namespace CryptoTools.Core.Interfaces
{
    public interface INotificationManager
    {
        Task<bool> SendNotification(string message);
        Task<bool> SendTemplatedNotification(string template, params object[] parameters);
    }
}
