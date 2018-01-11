using System.Collections.Generic;
using System.Linq;
using CryptoTools.Core.Indicators;
using CryptoTools.Core.Interfaces;
using CryptoTools.Core.Models;

namespace CryptoTools.Core.Strategies
{
    public class TheScalper : ITradingStrategy
    {
        public string Name => "The Scalper";

        public double? AdditionalValue { get; set; }

        public List<Candle> Candles { get; set; }

        public TheScalper()
        {
            this.Candles = new List<Candle>();
        }

        public List<int> Prepare()
        {
            var result = new List<int>();

            var stoch = Candles.Stoch();
            var sma200 = Candles.Sma(200);
            var closes = Candles.Select(x => x.Close).ToList();

            for (int i = 0; i < Candles.Count; i++)
            {
                if (i < 1)
                {
                    result.Add(0);
                }
                else
                {
                    if (sma200[i] < closes[i] && // Candles above the SMA
                        stoch.K[i - 1] <= stoch.D[i - 1] && // K below 20, oversold
                        stoch.K[i] > stoch.D[i] &&
                        stoch.D[i - 1] < 20 &&
                        stoch.K[i - 1] < 20 // && // K below 20, oversold
                    )
                    {
                        result.Add(1);
                    }
                    else
                    {
                        result.Add(0);
                    }
                }
            }

            AdditionalValue = (stoch.K[stoch.K.Count - 2]);

            return result;
        }
    }
}

