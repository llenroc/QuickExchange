using System.Collections.Generic;
using CryptoTools.Core.Models;

namespace CryptoTools.Core.Interfaces
{
    public interface ITradingStrategy
    {
        string Name { get; }
        double? AdditionalValue { get; set; }
        List<Candle> Candles { get; set; }
        List<int> Prepare();
    }
}
