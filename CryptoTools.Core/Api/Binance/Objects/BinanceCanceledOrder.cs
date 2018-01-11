﻿using Newtonsoft.Json;

namespace CryptoTools.Core.Api.Binance.Objects
{
    /// <summary>
    /// Information about a canceled order
    /// </summary>
    public class BinanceCanceledOrder
    {
        /// <summary>
        /// The symbol the order was for
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// The client order id the order was
        /// </summary>
        [JsonProperty("origClientOrderId")]
        public string OriginalClientOrderId { get; set; }
        /// <summary>
        /// The order id as generated by Binance
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// The new client order id
        /// </summary>
        public string ClientOrderId { get; set; }
    }
}
