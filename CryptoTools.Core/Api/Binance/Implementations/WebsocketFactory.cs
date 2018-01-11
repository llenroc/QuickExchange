using CryptoTools.Core.Api.Binance.Interfaces;
using WebSocket4Net;

namespace CryptoTools.Core.Api.Binance.Implementations
{
    public class WebsocketFactory : IWebsocketFactory
    {
        public IWebsocket CreateWebsocket(string url)
        {
            return new BinanceSocket(new WebSocket(url));
        }
    }
}
