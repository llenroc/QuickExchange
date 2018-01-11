using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CryptoTools.Core.Api.Bittrex.Models;

namespace CryptoTools.Core.Api.Bittrex
{
    public class BittrexApi
    {
        private readonly BittrexClient _api;

        public BittrexApi(string apiKey, string apiSecret)
        {
            _api = new BittrexClient(apiKey, apiSecret);
        }
        
        public async Task<string> Buy(string market, double quantity, double rate)
        {
            var result = await _api.BuyLimit(market, quantity, rate);

            if (!result.Success)
                throw new Exception($"Bittrex API failure {result.Message}");

            return result.Result.Uuid.ToString();
        }
        public async Task<string> Sell(string market, double quantity, double rate)
        {
            var result = await _api.SellLimit(market, quantity, rate);

            if (!result.Success)
                throw new Exception($"Bittrex API failure {result.Message}");

            return result.Result.Uuid.ToString();
        }

        public async Task CancelOrder(Guid orderId)
        {
            var result = await _api.CancelOrder(orderId);

            if (!result.Success)
                throw new Exception($"Bittrex API failure {result.Message}");
        }

        public async Task<double> GetBalance(string currency)
        {
            var result = await _api.GetBalance(currency);

            if(!result.Success)
                throw new Exception($"Bittrex API failure {result.Message}");

            return result.Result.Balance;
        }

        public async Task<List<Market>> GetMarkets()
        {
            var result = await _api.GetMarkets();

            if (!result.Success)
                throw new Exception($"Bittrex API failure {result.Message}");

            return result.Result;
        }
        public async Task<List<AccountBalance>> GetBalances()
        {
            var result = await _api.GetBalances();

            if (!result.Success)
                throw new Exception($"Bittrex API failure {result.Message}");

            return result.Result;
        }

        public async Task<List<MarketSummary>> GetMarketSummaries()
        {
            var result = await _api.GetMarketSummaries();

            if (!result.Success)
                throw new Exception($"Bittrex API failure {result.Message}");

            return result.Result;
        }

        public async Task<List<OpenOrder>> GetOpenOrders()
        {
            var result = await _api.GetOpenOrders();

            if (!result.Success)
                throw new Exception($"Bittrex API failure {result.Message}");

            return result.Result;
        }

        public async Task<List<OpenOrder>> GetOpenOrders(string market)
        {
            var result = await _api.GetOpenOrders(market);

            if (!result.Success)
                throw new Exception($"Bittrex API failure {result.Message}");

            return result.Result;
        }

        public string GetPairDetailUrl(string market)
        {
            return $"https://bittrex.com/Market/Index?MarketName={market}";
        }

        public async Task<Ticker> GetTicker(string market)
        {
            var result = await _api.GetTicker(market);

            if (!result.Success)
                throw new Exception($"Bittrex API failure {result.Message}");

            return result.Result;
        }
        
        public async Task<HistoricAccountOrder> GetOrder(string orderId)
        {
            var result = await _api.GetOrder(new Guid(orderId));

            if (!result.Success)
                throw new Exception($"Bittrex API failure {result.Message}");

            return result.Result;
        }

        public async Task<List<Core.Models.Candle>> GetTickerHistory(string market, long startDate, Models.Period period = Models.Period.FiveMin)
        {
            var result = await _api.GetTickerHistory(market, startDate, period);

            if (!result.Success)
                throw new Exception($"Bittrex API failure {result.Message}");

            return result.Result.ToGenericCandles();
        }

    }
}
