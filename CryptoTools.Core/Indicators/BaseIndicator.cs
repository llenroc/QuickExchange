using System.Collections.Generic;
using System.Linq;

namespace CryptoTools.Core.Indicators
{
    public static partial class Extensions
    {
        private static List<double?> FixIndicatorOrdering(List<double> items, int outBegIdx, int outNbElement)
        {
            var outValues = new List<double?>();
            var validAdx = items.Take(outNbElement);

            for (int i = 0; i < outBegIdx; i++)
                outValues.Add(null);

            foreach (var value in validAdx)
                outValues.Add(value);

            return outValues;
        }
    }
}
