namespace CryptoTools.Core.Api.Binance.Interfaces
{
    public interface IRequestFactory
    {
        IRequest Create(string uri);
    }
}
