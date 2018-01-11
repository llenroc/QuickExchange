using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Binance.Net;
using CryptoTools.Core.Api.Binance.Objects;
using TicTacTec.TA.Library;

namespace CryptoTools.Core.Api.Binance
{
    public class BinanceApi
    {
        private readonly BinanceClient _api;

        public BinanceApi(string apiKey, string apiSecret)
        {
            _api = new BinanceClient(apiKey, apiSecret);
        }

        public async Task<List<BinancePrice>> GetMarketSummaries()
        {
            var result = await _api.GetAllPricesAsync();

            if (!result.Success)
                throw new Exception($"Binance API failure {result.Error.Message}");

            return result.Data.ToList();
        }

        public async Task<BinanceCanceledOrder> CancelOrderAsync(string symbol, long orderId)
        {
            var result = await _api.CancelOrderAsync(symbol, orderId);

            if (!result.Success)
                throw new Exception($"Binance API failure {result.Error.Message}");

            return result.Data;
        }

        public async Task<BinanceOrder> GetOrder(string symbol, long orderId)
        {
            var result = await _api.GetAllOrdersAsync(symbol);

            if (!result.Success)
                throw new Exception($"Binance API failure {result.Error.Message}");

            return result.Data.FirstOrDefault(x => x.OrderId == orderId);
        }

        public async Task<List<BinanceOrder>> GetOpenOrdersAsync(string symbol)
        {
            var result = await _api.GetOpenOrdersAsync(symbol);

            if (!result.Success)
                throw new Exception($"Binance API failure {result.Error.Message}");

            return result.Data.ToList();
        }

        public async Task<List<BinanceOrder>> GetOrdersAsync(string symbol)
        {
            var result = await _api.GetAllOrdersAsync(symbol);

            if (!result.Success)
                throw new Exception($"Binance API failure {result.Error.Message}");

            return result.Data.ToList();
        }

        public async Task<List<BinanceOrder>> GetOpenOrdersAsync()
        {
            var result = await _api.GetOpenOrdersAsync();

            if (!result.Success)
                throw new Exception($"Binance API failure {result.Error.Message}");

            return result.Data.ToList();
        }

        public async Task<List<BinancePrice>> GetAllPricesAsync()
        {
            var result = await _api.GetAllPricesAsync();

            if (!result.Success)
                throw new Exception($"Binance API failure {result.Error.Message}");

            return result.Data.ToList();
        }

        public async Task<BinanceAccountInfo> GetAccountInfo()
        {
            var result = await _api.GetAccountInfoAsync();

            if (!result.Success)
                throw new Exception($"Binance API failure {result.Error.Message}");

            return result.Data;
        }

        public async Task<BinancePrice> GetTickerPrice(string symbol)
        {
            var result = await _api.GetTickerPriceAsync(symbol);

            if (!result.Success)
                throw new Exception($"Binance API failure {result.Error.Message}");

            return result.Data;
        }

        public async Task<List<Core.Models.Candle>> GetTickerHistory(string market, KlineInterval period, DateTime startTime)
        {
            var result = await _api.GetKlinesAsync(market, period, startTime);

            if (!result.Success)
                throw new Exception($"Binance API failure {result.Error.Message}");

            return result.Data.ToList().ToGenericCandles();
        }

        public async Task<BinancePlacedOrder> PlaceOrderAsync(string coin, OrderSide orderSide, OrderType orderType, TimeInForce inForce, decimal quantity, decimal price, string newClientOrderId = null, decimal? stopPrice = null, decimal? icebergQty = null)
        {
            var result = await _api.PlaceOrderAsync(coin, orderSide, orderType, inForce, quantity, price, newClientOrderId, stopPrice, icebergQty);

            if (!result.Success)
                throw new Exception($"Binance API failure {result.Error.Message}");

            return result.Data;
        }

        public async Task<BinanceExchangeInfo> GetExchangeInfo()
        {
            var result = await _api.GetExchangeInfoAsync();

            if (!result.Success)
                throw new Exception($"Binance API failure {result.Error.Message}");

            return result.Data;
        }
    }
}
