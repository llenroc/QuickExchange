using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoTools.Core.Api.BlockChain.Models;

namespace CryptoTools.QuickExchange
{
    public interface IExchangeManager
    {
        Task GetBalances(Prices prices);
        Task GetOpenOrders(Prices prices);
        Task Buy(string coin, double price, double quantity);
        Task CancelOrder(Prices prices, string coin = null);
        Task Sell(string coin, double price, double quantity);
    }
}
