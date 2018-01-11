﻿using System.Collections.Generic;
using CryptoTools.Core.Indicators;
using CryptoTools.Core.Interfaces;
using CryptoTools.Core.Models;

namespace CryptoTools.Core.Strategies
{
    public class EmaStochRsi : ITradingStrategy
    {
        public string Name => "EMA Stoch RSI";

        public List<Candle> Candles { get; set; }

        public EmaStochRsi()
        {
            this.Candles = new List<Candle>();
        }

        public List<int> Prepare()
        {
            var result = new List<int>();

            var stoch = Candles.Stoch(14);
            var ema5 = Candles.Ema(5);
            var ema10 = Candles.Ema(10);
            var rsi = Candles.Rsi(14);

            for (int i = 0; i < Candles.Count; i++)
            {
                if (i < 1)
                    result.Add(0);
                else
                {
                    var slowk1 = stoch.K[i];
                    var slowkp = stoch.K[i - 1];
                    var slowd1 = stoch.D[i];
                    var slowdp = stoch.D[i - 1];

                    bool pointedUp = false, pointedDown = false, kUp = false, dUp = false;

                    if (slowkp < slowk1) kUp = true;
                    if (slowdp < slowd1) dUp = true;
                    if (slowkp < 80 && slowdp < 80 && kUp && dUp) pointedUp = true;
                    if (slowkp > 20 && slowdp > 20 && !kUp && !dUp) pointedDown = true;

                    if (ema5[i] >= ema10[i] && ema5[i - 1] < ema10[i] && rsi[i] > 50 && pointedUp)
                        result.Add(1);
                    else if (ema5[i] <= ema10[i] && ema5[i - 1] > ema10[i] && rsi[i] < 50 && pointedDown)
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