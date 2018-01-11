using System.Collections.Generic;

namespace CryptoTools.Core.Models
{
    public class MarketData
    {
        public string Name { get; set; }
        public List<Candle> Candles { get; set; }
        public List<int> Trend { get; set; }
    }
}
