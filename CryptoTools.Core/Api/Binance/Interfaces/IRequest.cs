using System.Net;

namespace CryptoTools.Core.Api.Binance.Interfaces
{
    public interface IRequest
    {
        WebHeaderCollection Headers { get; set; }
        string Method { get; set; }

        IResponse GetResponse();
    }
}
