using System;

namespace CryptoTools.Core.Models
{
    public class Balance 
    {
        public double TotalBalance { get; set; }
        public double Profit { get; set; }
        public DateTime? BalanceDate { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
