using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoTools.Core.Api.Bittrex;
using CryptoTools.Core.Api.Bittrex.Models;
using CryptoTools.Core.Api.BlockChain.Models;

namespace CryptoTools.QuickExchange
{
    public class BittrexManager : IExchangeManager
    {
        private readonly BittrexApi _bittrexApi;

        public BittrexManager()
        {
            _bittrexApi = new BittrexApi(Properties.Settings.Default.BittrexApiKey, Properties.Settings.Default.BittrexApiSecret);
        }

        public async Task GetBalances(Prices prices)
        {
            var myPrices = new List<Tuple<string, double, double>>();

            // Get the Bittrex balances.
            var accountBalances = await _bittrexApi.GetBalances();

            // Check if there's an actual balance in there.
            if (accountBalances.Any(x => x.Balance > 0))
            {
                // Loop actual balances.
                foreach (var item in accountBalances.Where(x => x.Balance > 0))
                {
                    // If the balance is not BTC, we need to get the BTC value from Bittrex.
                    if (item.Currency != "BTC")
                    {
                        try
                        {
                            // Get the current value.
                            var ticker = await _bittrexApi.GetTicker("BTC-" + item.Currency);

                            if (ticker == null)
                                myPrices.Add(new Tuple<string, double, double>(item.Currency, 0, 0));
                            else
                                myPrices.Add(new Tuple<string, double, double>(item.Currency,
                                    item.Balance * ticker.Last * (double) prices.Eur.Last,
                                    item.Balance * ticker.Last * (double) prices.Usd.Last));
                        }
                        catch (Exception)
                        {
                            myPrices.Add(new Tuple<string, double, double>(item.Currency, 0, 0));
                        }
                    }
                    else
                    {
                        // BTC can simply be added as is.
                        myPrices.Add(new Tuple<string, double, double>(item.Currency,
                            item.Balance * (double)prices.Eur.Last,
                            item.Balance * (double)prices.Usd.Last));
                    }
                }

                Console.WriteLine(accountBalances.Where(x => x.Balance > 0)
                    .ToStringTable<AccountBalance>(
                        new[] { "Currency", "Available", "Balance", "Estimated EUR Value", "Estimated USD Value" },
                        x => x.Currency, x => x.Available.ToString("0.00000000"),
                        x => x.Balance.ToString("0.00000000"), x => myPrices.FirstOrDefault(y => y.Item1 == x.Currency)?.Item2.ToString("0.00 EUR"),
                        x => (myPrices.FirstOrDefault(y => y.Item1 == x.Currency)?.Item3.ToString("0.00 USD"))));

                Console.WriteLine($"\tTOTAL BALANCE: {(myPrices.Sum(x => x.Item2)):0.00} EUR / " + $"{(myPrices.Sum(x => x.Item3)):0.00} USD");
            }
            else
            {
                Console.WriteLine("\tNo balances found...");
            }
        }

        public async Task GetOpenOrders(Prices prices)
        {
            var orders = await _bittrexApi.GetOpenOrders();

            if (orders.Count > 0)
            {
                Console.WriteLine(orders
                    .ToStringTable<OpenOrder>(
                        new[]
                        {
                                "Market", "Bid/Ask", "Quantity", "Remaining", "Estimated Total", "Estimated EUR Value",
                                "Estimated USD Value"
                        },
                        x => x.Exchange, x => x.Limit.ToString("0.00000000"), x => x.Quantity,
                        x => x.QuantityRemaining, x => ((x.Limit * x.Quantity) * 0.9975).ToString("0.00000000"),
                        x => (((x.Limit * x.Quantity) * 0.9975) * (double)prices.Eur.Last).ToString("0.00 EUR"),
                        x => (((x.Limit * x.Quantity) * 0.9975) * (double)prices.Usd.Last).ToString("0.00 USD")));
            }
            else
            {
                Console.WriteLine("\tNo open orders found...");
            }

            Console.WriteLine(
                $"\tTOTAL OPEN ORDERS: {(orders.Sum(x => x.Limit * x.Quantity) * (double)prices.Eur.Last):0.00} EUR / " +
                $"{(orders.Sum(x => x.Limit * x.Quantity) * (double)prices.Usd.Last):0.00} USD", ConsoleColor.Cyan);
        }

        public async Task CancelOrder(Prices prices, string coin = null)
        {
            var orders = await _bittrexApi.GetOpenOrders();

            if (coin != null)
                orders = orders.Where(x => x.Exchange.Replace("BTC-", string.Empty) == coin).ToList();

            if (orders.Count > 0)
            {
                Console.WriteLine(orders
                    .ToStringTable<OpenOrder>(
                        new[]
                        {
                                "ID", "Market", "Bid/Ask", "Quantity", "Remaining", "Estimated Total",
                                "Estimated EUR Value", "Estimated USD Value"
                        },
                        x => orders.IndexOf(x) + 1, x => x.Exchange, x => x.Limit.ToString("0.00000000"), x => x.Quantity,
                        x => x.QuantityRemaining, x => ((x.Limit * x.Quantity) * 0.9975).ToString("0.00000000"),
                        x => (((x.Limit * x.Quantity) * 0.9975) * (double)prices.Eur.Last).ToString("0.00 EUR"),
                        x => (((x.Limit * x.Quantity) * 0.9975) * (double)prices.Usd.Last).ToString("0.00 USD")));


                Console.Write("\tWhich order should be cancelled (ID) (or enter -1 for all): ");
                int orderId = 0;
                if (int.TryParse(Console.ReadLine(), out orderId))
                {
                    if (orderId > 0 && orderId <= orders.Count)
                    {
                        ConsoleHelpers.WriteColored("\tWARNING: GOING TO CANCEL AN ORDER, DO YOU WANT TO CONTINUE? (YES/NO) ",
                            ConsoleColor.Yellow);

                        if (Console.ReadLine()?.ToLower() == "yes")
                        {
                            await _bittrexApi.CancelOrder(orders[orderId - 1].OrderUuid);
                            ConsoleHelpers.WriteColoredLine($"\tOrder {orders[orderId - 1].OrderUuid} cancelled.", ConsoleColor.Green);
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
                            foreach (var order in orders)
                            {
                                await _bittrexApi.CancelOrder(order.OrderUuid);
                                ConsoleHelpers.WriteColoredLine($"\tOrder {order.OrderUuid} cancelled.", ConsoleColor.Green);
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
                await _bittrexApi.Buy("BTC-" + coin.ToUpper(), quantity, price);
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
                await _bittrexApi.Sell("BTC-"+coin.ToUpper(), quantity, price);
                ConsoleHelpers.WriteColoredLine("\tSell order placed.", ConsoleColor.Green);
            }
            else
            {
                ConsoleHelpers.WriteColoredLine("\tSell cancelled.", ConsoleColor.Red);
            }
        }
    }
}
