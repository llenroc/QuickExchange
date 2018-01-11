namespace CryptoTools.Core.Api.BlockChain.Models
{
    public class Prices
    {
        public Price Eur { get; set; }
        public Price Usd { get; set; }
    }

    public class Price
    {
        public decimal _15m { get; set; }
        public decimal Last { get; set; }
        public decimal Buy { get; set; }
        public decimal Sell { get; set; }
        public string Symbol { get; set; }
    }
}
