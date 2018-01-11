using CryptoTools.Core.Api.Binance.Converters;
using Newtonsoft.Json;
using System;

namespace CryptoTools.Core.Api.Binance.Objects
{
    /// <summary>
    /// A event received by a Binance websocket
    /// </summary>
    public class BinanceStreamEvent
    {
        /// <summary>
        /// The type of the event
        /// </summary>
        [JsonProperty("e")]
        public string Event { get; set; }
        /// <summary>
        /// The time the event happened
        /// </summary>
        [JsonProperty("E"), JsonConverter(typeof(TimestampConverter))]
        public DateTime EventTime { get; set; }
    }
}
