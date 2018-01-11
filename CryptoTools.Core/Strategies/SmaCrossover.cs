﻿using System.Collections.Generic;
using CryptoTools.Core.Indicators;
using CryptoTools.Core.Interfaces;
using CryptoTools.Core.Models;

namespace CryptoTools.Core.Strategies
{
    public class SmaCrossover : ITradingStrategy
    {
        public string Name => "SMA Crossover";
        public List<Candle> Candles { get; set; }

        public SmaCrossover()
        {
            this.Candles = new List<Candle>();
        }

        public List<int> Prepare()
        {
            var result = new List<int>();

            var sma12 = Candles.Sma(12);
            var sma26 = Candles.Sma(26);

            for (int i = 0; i < Candles.Count; i++)
            {
                // Since we look back 1 candle, the first candle can never be a signal.
                if (i == 0)
                    result.Add(0);
                // When the fast SMA moves above the slow SMA, we have a positive cross-over
                else if (sma12[i] < sma26[i] && sma12[i - 1] > sma26[i])
                    result.Add(1);
                // When the slow SMA moves above the fast SMA, we have a negative cross-over
                else if (sma12[i] > sma26[i] && sma12[i - 1] < sma26[i])
                    result.Add(-1);
                else
                    result.Add(0);
            }

            return result;
        }

        public double? AdditionalValue { get; set; }
    }
}