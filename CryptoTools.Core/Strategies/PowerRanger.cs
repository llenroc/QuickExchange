using System.Collections.Generic;
using CryptoTools.Core.Indicators;
using CryptoTools.Core.Interfaces;
using CryptoTools.Core.Models;

namespace CryptoTools.Core.Strategies
{
    public class PowerRanger : ITradingStrategy
    {
        public string Name => "Power Ranger";

        public List<Candle> Candles { get; set; }

        public PowerRanger()
        {
            this.Candles = new List<Candle>();
        }

        public List<int> Prepare()
        {
            var result = new List<int>();
            var stoch = Candles.Stoch(10);

            for (int i = 0; i < Candles.Count; i++)
            {
                if (i < 1)
                    result.Add(0);
                else
                {
                    if ((stoch.K[i] > 20 && stoch.K[i - 1] < 20) || (stoch.D[i] > 20 && stoch.D[i - 1] < 20))
                        result.Add(1);
                    else if ((stoch.K[i] < 80 && stoch.K[i - 1] > 80) || (stoch.D[i] < 80 && stoch.D[i - 1] > 80))
                        result.Add(-1);
                    else
                        result.Add(0);
                }
            }

            return result;
        }

        public double? AdditionalValue { get; set; }
    }
}
