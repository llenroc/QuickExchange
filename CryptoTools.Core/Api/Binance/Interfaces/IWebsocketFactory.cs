namespace CryptoTools.Core.Api.Binance.Interfaces
{
    public interface IWebsocketFactory
    {
        IWebsocket CreateWebsocket(string url);
    }
}
