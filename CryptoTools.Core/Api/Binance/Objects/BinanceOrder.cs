﻿using System;
using CryptoTools.Core.Api.Binance.Converters;
using Newtonsoft.Json;

namespace CryptoTools.Core.Api.Binance.Objects
{
    /// <summary>
    /// Information regarding a specific order
    /// </summary>
    public class BinanceOrder
    {
        /// <summary>
        /// The symbol the order is for
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// The order id generated by Binance
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// The order id as assigned by the client
        /// </summary>
        public string ClientOrderId { get; set; }
        /// <summary>
        /// The price of the order
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// The original quantity of the order
        /// </summary>
        [JsonProperty("origQty")]
        public decimal OriginalQuantity { get; set; }
        /// <summary>
        /// The currently executed quantity of the order
        /// </summary>
        [JsonProperty("executedQty")]
        public decimal ExecutedQuantity { get; set; }
        /// <summary>
        /// The status of the order
        /// </summary>
        [JsonConverter(typeof(OrderStatusConverter))]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// How long the order is active
        /// </summary>
        [JsonConverter(typeof(TimeInForceConverter))]
        public TimeInForce TimeInForce { get; set; }
        /// <summary>
        /// The type of the order
        /// </summary>
        [JsonConverter(typeof(OrderTypeConverter))]
        public OrderType Type { get; set; }
        /// <summary>
        /// The side of the order
        /// </summary>
        [JsonConverter(typeof(OrderSideConverter))]
        public OrderSide Side { get; set; }
        /// <summary>
        /// The stop price
        /// </summary>
        public decimal StopPrice { get; set; }
        /// <summary>
        /// The iceberg quantity
        /// </summary>
        [JsonProperty("icebergQty")]
        public decimal IcebergQuantity { get; set; }
        /// <summary>
        /// The time the order was submitted
        /// </summary>
        [JsonConverter(typeof(TimestampConverter))]
        public DateTime Time { get; set; }
    }
}
