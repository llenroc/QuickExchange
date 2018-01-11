using System;
using CryptoTools.Core.Api.Binance.Converters;
using Newtonsoft.Json;

namespace CryptoTools.Core.Api.Binance.Objects
{
    public class BinanceCheckTime
    {
        [JsonProperty("serverTime"), JsonConverter(typeof(TimestampConverter))]
        public DateTime ServerTime { get; set; }
    }
}
