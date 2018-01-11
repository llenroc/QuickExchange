using System.IO;

namespace CryptoTools.Core.Api.Binance.Interfaces
{
    public interface IResponse
    {
        Stream GetResponseStream();
    }
}
