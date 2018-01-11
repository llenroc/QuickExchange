﻿using System.Collections.Generic;
using System.Linq;
using CryptoTools.Core.Indicators;
using CryptoTools.Core.Interfaces;
using CryptoTools.Core.Models;

namespace CryptoTools.Core.Strategies
{
    public class FreqTrade : ITradingStrategy
    {
        public string Name => "FreqTrade";

        public List<Candle> Candles { get; set; }

        public List<int> Prepare()
        {
            var result = new List<int>();

            var sma = Candles.Sma(100);
            var closes = Candles.Select(x => x.Close).ToList();
            var adx = Candles.Adx();
            var tema = Candles.Tema(4);
            var mfi = Candles.Mfi(14);
            var sar = Candles.Sar(0.02, 0.22);

            var cci = Candles.Cci(5);
            var stoch = Candles.StochFast();
            var bbandsLower = Candles.Bbands().MiddleBand;
            var fishers = Candles.Fisher();

            for (int i = 0; i < Candles.Count; i++)
            {
                if (closes[i] < sma[i] && cci[i] < -100 && stoch.D[i] < 20 && fishers[i] < 0 &&
                    adx[i] > 20 && mfi[i] < 30 && tema[i] <= bbandsLower[i])
                    result.Add(1);
                else if (fishers[i] == 1)
                    result.Add(-1);
                else
                    result.Add(0);
            }

            return result;
        }

        public double? AdditionalValue { get; set; }
    }
}
