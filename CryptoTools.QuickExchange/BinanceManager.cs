using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoTools.Core.Api.Binance;
using CryptoTools.Core.Api.Binance.Objects;
using CryptoTools.Core.Api.BlockChain.Models;

namespace CryptoTools.QuickExchange
{
    public class BinanceManager : IExchangeManager
    {
        private readonly BinanceApi _binanceApi;

        public BinanceManager()
        {
            _binanceApi = new BinanceApi(Properties.Settings.Default.BinanceApiKey, Properties.Settings.Default.BinanceApiSecret);
        }

        public async Task GetBalances(Prices prices)
        {
            var myPrices = new List<Tuple<string, decimal, decimal>>();

            // Get the Binance balances
            var result = await _binanceApi.GetAccountInfo();
            var accountBalances = result.Balances.Where(x => x.Total > 0).ToList();

            if (accountBalances.Count > 0)
            {
                var priceResult = await _binanceApi.GetAllPricesAsync();

                foreach (var value in accountBalances)
                {
                    try
                    {
                        if (value.Asset == "BTC")
                        {
                            myPrices.Add(new Tuple<string, decimal, decimal>(value.Asset,
                                value.Total * prices.Eur.Last,
                                value.Total * prices.Usd.Last));
                        }
                        else
                        {
                            var currentPrice = priceResult.FirstOrDefault(y => y.Symbol == value.Asset + "BTC");

                            if (currentPrice == null)
                                myPrices.Add(new Tuple<string, decimal, decimal>(value.Asset, 0, 0));
                            else
                                myPrices.Add(new Tuple<string, decimal, decimal>(value.Asset,
                                    value.Total * currentPrice.Price * prices.Eur.Last,
                                    value.Total * currentPrice.Price * prices.Usd.Last));
                        }
                    }
                    catch (Exception)
                    {
                        myPrices.Add(new Tuple<string, decimal, decimal>(value.Asset, 0, 0));
                    }
                }

                Console.WriteLine(accountBalances.ToStringTable<BinanceBalance>(
                    new[] { "Currency", "Available", "Balance", "Estimated EUR Value", "Estimated USD Value" },
                    x => x.Asset, x => x.Free.ToString("0.00000000"),
                    x => x.Total.ToString("0.00000000"), x => myPrices.FirstOrDefault(y => y.Item1 == x.Asset)?.Item2.ToString("0.00 EUR"),
                    x => (myPrices.FirstOrDefault(y => y.Item1 == x.Asset)?.Item3.ToString("0.00 USD"))));


                Console.WriteLine($"\tTOTAL BALANCE: {(myPrices.Sum(x => x.Item2)):0.00} EUR / " + $"{(myPrices.Sum(x => x.Item3)):0.00} USD");
            }
            else
            {
                Console.WriteLine("No balances found...");
            }
        }

        public async Task CancelOrder(Prices prices, string coin = null)
        {
            var openOrders = await _binanceApi.GetOpenOrdersAsync();

            if (coin != null)
                openOrders = openOrders.Where(x => x.Symbol.Replace("BTC", string.Empty) == coin.ToUpper()).ToList();

            if (openOrders.Count > 0)
            {
                Console.WriteLine(openOrders
                    .ToStringTable<BinanceOrder>(
                        new[]
                        {
                                "Order ID", "Market", "Bid/Ask", "Quantity", "Remaining", "Estimated Total",
                                "Estimated EUR Value",
                                "Estimated USD Value"
                        },
                        x => openOrders.IndexOf(x) + 1,
                        x => x.Symbol, x => x.Price.ToString("0.00000000"), x => x.OriginalQuantity,
                        x => x.OriginalQuantity - x.ExecutedQuantity,
                        x => ((x.Price * x.OriginalQuantity)).ToString("0.00000000"),
                        x => (((x.Price * x.OriginalQuantity)) * prices.Eur.Last).ToString("0.00 EUR"),
                        x => (((x.Price * x.OriginalQuantity)) * prices.Usd.Last).ToString("0.00 USD")));

                Console.Write("\tWhich order should be cancelled (ID) (or enter -1 for all): ");
                int orderId = 0;

                if (int.TryParse(Console.ReadLine(), out orderId))
                {
                    if (orderId > 0 && orderId <= openOrders.Count)
                    {
                        ConsoleHelpers.WriteColored("\tWARNING: GOING TO CANCEL AN ORDER, DO YOU WANT TO CONTINUE? (YES/NO) ", ConsoleColor.Yellow);

                        if (Console.ReadLine()?.ToLower() == "yes")
                        {
                            await _binanceApi.CancelOrderAsync(openOrders[orderId - 1].Symbol, openOrders[orderId - 1].OrderId);
                            ConsoleHelpers.WriteColoredLine($"\tOrder {openOrders[orderId - 1].OrderId} cancelled.", ConsoleColor.Green);
                        }
                        else
                        {
                            ConsoleHelpers.WriteColoredLine("\tCancel cancelled (cancelception?).", ConsoleColor.Red);
                        }
                    }
                    else if (orderId == -1)
                    {
                        ConsoleHelpers.WriteColored("\tWARNING: GOING TO CANCEL ALL OF THE ABOVE ORDERS, DO YOU WANT TO CONTINUE? (YES/NO) ", ConsoleColor.Yellow);

                        if (Console.ReadLine()?.ToLower() == "yes")
                        {
                            foreach (var order in openOrders)
                            {
                                await _binanceApi.CancelOrderAsync(order.Symbol, order.OrderId);
                                ConsoleHelpers.WriteColoredLine($"\tOrder {order.OrderId} cancelled.", ConsoleColor.Green);
                            }
                        }
                        else
                        {
                            ConsoleHelpers.WriteColoredLine("\tCancel cancelled (cancelception?).", ConsoleColor.Red);
                        }
                    }
                    else
                    {
                        ConsoleHelpers.WriteColoredLine("\tInvalid order ID.", ConsoleColor.Red);
                    }
                }
                else
                {
                    ConsoleHelpers.WriteColoredLine("\tInvalid order ID.", ConsoleColor.Red);
                }
            }
            else
            {
                Console.WriteLine("\tNo open orders found...");
            }
        }

        public async Task GetOpenOrders(Prices prices)
        {
            var openOrders = await _binanceApi.GetOpenOrdersAsync();

            if (openOrders.Count > 0)
            {
                Console.WriteLine(openOrders
                    .ToStringTable<BinanceOrder>(
                        new[]
                        {
                                        "Market", "Bid/Ask", "Quantity", "Remaining", "Estimated Total",
                                        "Estimated EUR Value",
                                        "Estimated USD Value"
                        },
                        x => x.Symbol, x => x.Price.ToString("0.00000000"), x => x.OriginalQuantity,
                        x => x.OriginalQuantity - x.ExecutedQuantity, x => ((x.Price * x.OriginalQuantity)).ToString("0.00000000"),
                        x => (((x.Price * x.OriginalQuantity)) * prices.Eur.Last).ToString("0.00 EUR"),
                        x => (((x.Price * x.OriginalQuantity)) * prices.Usd.Last).ToString("0.00 USD")));
            }
            else
            {
                Console.WriteLine("\tNo open orders found...");
            }

            Console.WriteLine(
                $"\tTOTAL OPEN ORDERS: {(openOrders.Sum(x => x.Price * x.OriginalQuantity) * prices.Eur.Last):0.00} EUR / " +
                $"{(openOrders.Sum(x => x.Price * x.OriginalQuantity) * prices.Usd.Last):0.00} USD", ConsoleColor.Cyan);
        }

        public async Task Buy(string coin, double price, double quantity)
        {
            // Convert to SAT.
            if (price > 1)
            {
                price = price / 100000000;
            }

            ConsoleHelpers.WriteColored($"\tWARNING: GOING TO BUY {quantity:0.00000000} {coin} at a rate of {price:0.00000000}, " +
                                        $"DO YOU WANT TO CONTINUE? (YES/NO) ", ConsoleColor.Yellow);

            if (Console.ReadLine()?.ToLower() == "yes")
            {
                var result = await _binanceApi.PlaceOrderAsync(coin.ToUpper() + "BTC", OrderSide.Buy, OrderType.Limit,
                    TimeInForce.GoodTillCancel, (decimal)quantity, (decimal)price);
                ConsoleHelpers.WriteColoredLine("\tBuy order placed.", ConsoleColor.Green);
            }
            else
            {
                ConsoleHelpers.WriteColoredLine("\tBuy cancelled.", ConsoleColor.Red);
            }
        }

        public async Task Sell(string coin, double price, double quantity)
        {
            if (price > 1)
            {
                price = price / 100000000;
            }

            ConsoleHelpers.WriteColored($"\tWARNING: GOING TO SELL {quantity:0.00000000} {coin} at a rate of {price:0.00000000}, " +
                                        $"DO YOU WANT TO CONTINUE? (YES/NO) ", ConsoleColor.Yellow);

            if (Console.ReadLine()?.ToLower() == "yes")
            {
                var result = await _binanceApi.PlaceOrderAsync(coin.ToUpper() + "BTC", OrderSide.Sell, OrderType.Limit, TimeInForce.GoodTillCancel, (decimal)quantity, (decimal)price);
                ConsoleHelpers.WriteColoredLine("\tSell order placed.", ConsoleColor.Green);
            }
            else
            {
                ConsoleHelpers.WriteColoredLine("\tSell cancelled.", ConsoleColor.Red);
            }
        }
    }
}
