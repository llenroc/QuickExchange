using System.Collections.Generic;
using System.Linq;

namespace CryptoTools.Core.Api.Binance
{
    public static class Extensions
    {
        public static List<Core.Models.Candle> ToGenericCandles(this List<Objects.BinanceKline> candles)
        {
            return candles.Select(x => new Core.Models.Candle
            {
                Close = (double)x.Close,
                High = (double)x.High,
                Low = (double)x.Low,
                Open = (double)x.Open,
                Timestamp = x.OpenTime,
                Volume = (double)x.Volume
            }).ToList();
        }
    }
}
