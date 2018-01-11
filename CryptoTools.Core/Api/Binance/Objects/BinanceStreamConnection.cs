using CryptoTools.Core.Api.Binance.Interfaces;

namespace CryptoTools.Core.Api.Binance.Objects
{
    public class BinanceStream
    {
        public IWebsocket Socket { get; set; }
        public int StreamId { get; set; }
        public bool UserStream { get; set; }
    }
}
