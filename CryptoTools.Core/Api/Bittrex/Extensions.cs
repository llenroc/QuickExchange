using System.Collections.Generic;
using System.Linq;

namespace CryptoTools.Core.Api.Bittrex
{
    public static class Extensions
    {
        public static List<Core.Models.Candle> ToGenericCandles(this List<Models.Candle> candles)
        {
            return candles.Select(x => new Core.Models.Candle
            {
                Close = x.C,
                High = x.H,
                Low = x.L,
                Open = x.O,
                Timestamp = x.T,
                Volume = x.V
            }).ToList();
        }
    }
}
