using System.Collections.Generic;
using System.Linq;
using CryptoTools.Core.Indicators;
using CryptoTools.Core.Interfaces;
using CryptoTools.Core.Models;

namespace CryptoTools.Core.Strategies
{
    public class BigThreeTrend : ITradingStrategy
    {
        public string Name => "Big Three Trend";

        public List<Candle> Candles { get; set; }

        public BigThreeTrend()
        {
            this.Candles = new List<Candle>();
        }

        public List<int> Prepare()
        {
            var result = new List<int>();

            var sma20 = Candles.Sma(20);
            var sma40 = Candles.Sma(40);
            var sma80 = Candles.Sma(80);

            for (int i = 0; i < Candles.Count; i++)
            {
                if (i < 1)
                {
                    result.Add(0);
                }
                else
                {
                    var previousIsRed = Candles[i].Close < Candles[i].Open;
                    var beforeIsGreen = Candles[i - 1].Close > Candles[i - 1].Open;

                    var highestSma = new List<double?> { sma20[i], sma40[i], sma80[i] }.Max();

                    var lastAboveSma = Candles[i].Close > highestSma && Candles[i].High > highestSma &&
                                       Candles[i].Low > highestSma && Candles[i].Open > highestSma;

                    var previousAboveSma = Candles[i - 1].Close > highestSma && Candles[i - 1].High > highestSma &&
                                       Candles[i - 1].Low > highestSma && Candles[i - 1].Open > highestSma;

                    var allAboveSma = lastAboveSma && previousAboveSma;

                    if (previousIsRed && beforeIsGreen && allAboveSma && sma20[i] > sma40[i] && sma20[i] > sma80[i])
                        result.Add(1);
                    else
                        result.Add(0);
                }
            }

            return result;
        }

        public double? AdditionalValue { get; set; }
    }
}
