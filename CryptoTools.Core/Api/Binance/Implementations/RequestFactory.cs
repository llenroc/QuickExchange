using CryptoTools.Core.Api.Binance.Interfaces;
using System.Net;

namespace CryptoTools.Core.Api.Binance.Implementations
{
    public class RequestFactory : IRequestFactory
    {
        public IRequest Create(string uri)
        {
            return new Request(WebRequest.Create(uri));
        }
    }
}
