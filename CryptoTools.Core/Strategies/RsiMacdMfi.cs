using System.Collections.Generic;
using System.Linq;
using CryptoTools.Core.Indicators;
using CryptoTools.Core.Interfaces;
using CryptoTools.Core.Models;

namespace CryptoTools.Core.Strategies
{
    public class RsiMacdMfi : ITradingStrategy
    {
        public string Name => "RSI MACD MFI";

        public List<Candle> Candles { get; set; }

        public RsiMacdMfi()
        {
            this.Candles = new List<Candle>();
        }

        public List<int> Prepare()
        {
            var result = new List<int>();

            var macd = Candles.Macd(5,10,4);
            var rsi = Candles.Rsi(16);
            var mfi = Candles.Mfi();
            var ao = Candles.AwesomeOscillator();

            var close = Candles.Select(x => x.Close).ToList();

            for (int i = 0; i < Candles.Count; i++)
            {       
                    if (mfi[i] <30 && rsi[i] < 45 && ao[i] > 0)
                        result.Add(1);
                    else if ( mfi[i] > 30 && rsi[i] > 45 && ao[i] < 0)
                        result.Add(-1);
                    else
                        result.Add(0);
             }

            return result;
        }

        public double? AdditionalValue { get; set; }
    }
}