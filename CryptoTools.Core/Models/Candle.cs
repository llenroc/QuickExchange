﻿using System;

namespace CryptoTools.Core.Models
{
    public class Candle
    {
        public DateTime Timestamp { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }
    }
}
