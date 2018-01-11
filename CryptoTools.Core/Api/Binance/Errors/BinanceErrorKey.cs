namespace CryptoTools.Core.Api.Binance.Errors
{
    public enum BinanceErrorKey
    {
        NoApiCredentialsProvided,
        NoListenKey,
        MissingRequiredParameter,

        ErrorWeb,

        ParseErrorReader,
        ParseErrorSerialization,
        CantConnectToServer,

        UnknownError
    }
}
