﻿using System.Collections.Generic;
using System.Linq;
using CryptoTools.Core.Indicators;
using CryptoTools.Core.Interfaces;
using CryptoTools.Core.Models;

namespace CryptoTools.Core.Strategies
{
    public class SarStoch : ITradingStrategy
    {
        public string Name => "SAR Stoch";

        public List<Candle> Candles { get; set; }

        public SarStoch()
        {
            this.Candles = new List<Candle>();
        }

        public List<int> Prepare()
        {
            var result = new List<int>();

            var stoch = Candles.Stoch(13);
            var stochFast = Candles.StochFast(13);
            var sar = Candles.Sar(3);
            var highs = Candles.Select(x => x.High).ToList();
            var lows = Candles.Select(x => x.Low).ToList();
            var closes = Candles.Select(x => x.Close).ToList();
            var opens = Candles.Select(x => x.Open).ToList();

            for (int i = 0; i < Candles.Count; i++)
            {
                if (i > 2)
                {
                    var currentSar = sar[i];
                    var priorSar = sar[i - 1];
                    var lastHigh = highs[i];
                    var lastLow = lows[i];
                    var lastOpen = opens[i];
                    var lastClose = closes[i];
                    var priorHigh = highs[i - 1];
                    var priorLow = lows[i - 1];
                    var priorOpen = opens[i - 1];
                    var priorClose = closes[i - 1];
                    var prevOpen = opens[i - 2];
                    var prevClose = closes[i - 2];

                    var below = currentSar < lastLow;
                    var above = currentSar > lastHigh;
                    var redCandle = lastOpen < lastClose;
                    var greenCandle = lastOpen > lastClose;
                    var priorBelow = priorSar < priorLow;
                    var priorAbove = priorSar > priorHigh;
                    var priorRedCandle = priorOpen < priorClose;
                    var priorGreenCandle = priorOpen > priorClose;
                    var prevRedCandle = prevOpen < prevClose;
                    var prevGreenCandle = prevOpen > prevClose;

                    priorRedCandle = (prevRedCandle || priorRedCandle);
                    priorGreenCandle = (prevGreenCandle || priorGreenCandle);

                    var fsar = 0;

                    if ((priorAbove && priorRedCandle) && (below && greenCandle))
                        fsar = 1;
                    else if ((priorBelow && priorGreenCandle) && (above && redCandle))
                        fsar = -1;

                    if (fsar == -1 && (stoch.K[i] > 90 || stoch.D[i] > 90 || stochFast.K[i] > 90 || stochFast.D[i] > 90))
                        result.Add(-1);
                    else if (fsar == 1 && (stoch.K[i] < 10 || stoch.D[i] < 10 || stochFast.K[i] < 10 || stochFast.D[i] < 10))
                        result.Add(1);
                    else
                        result.Add(0);
                }
                else
                {
                    result.Add(0);
                }
            }

            return result;
        }

        public double? AdditionalValue { get; set; }
    }
}
