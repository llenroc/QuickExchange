﻿using System.Collections.Generic;
using System.Linq;
using CryptoTools.Core.Indicators;
using CryptoTools.Core.Interfaces;
using CryptoTools.Core.Models;

namespace CryptoTools.Core.Strategies
{
    /// <summary>
    /// https://www.tradingview.com/script/zopumZ8a-Bollinger-RSI-Double-Strategy-by-ChartArt/
    /// </summary>
    public class BbandRsi : ITradingStrategy
    {
        public string Name => "BBand RSI";

        public List<Candle> Candles { get; set; }

        public BbandRsi()
        {
            this.Candles = new List<Candle>();
        }

        public List<int> Prepare()
        {
            var result = new List<int>();

            var currentPrices = Candles.Select(x => x.Close).ToList();
            var bbands = Candles.Bbands(20);
            var rsi = Candles.Rsi(16);
            
            for (int i = 0; i < Candles.Count; i++)
            {
                if (i == 0)
                    result.Add(0);
                else if (rsi[i] < 30 && currentPrices[i] < bbands.LowerBand[i])
                    result.Add(1);
                else if (rsi[i] > 70)
                    result.Add(-1);
                else
                    result.Add(0);
            }

            return result;
        }

        public double? AdditionalValue { get; set; }
    }
}
