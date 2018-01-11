using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommandLine;
using CryptoTools.Core.Api.BlockChain;
using CryptoTools.Core.Models;
using NDesk.Options;

namespace CryptoTools.QuickExchange
{
    class Program
    {
        private static IExchangeManager _binanceManager;
        private static IExchangeManager _bittrexManager;

        static void Main(string[] args)
        {
            try
            {
                _binanceManager = new BinanceManager();
                _bittrexManager = new BittrexManager();

                ConsoleHelpers.WriteSeparator();
                ConsoleHelpers.WriteIntro();
                ConsoleHelpers.WriteSeparator();
                ConsoleHelpers.WriteDonation();
                ConsoleHelpers.WriteSeparator();
                Console.WriteLine("\tPlease type your commands, type \"-h\" or \"--help\" to see an overview of all possible commands.");
                ShowMenu().Wait();
            }
            catch (Exception ex)
            {
                ConsoleHelpers.WriteColoredLine($"\tAn error occurred: {ex.InnerException?.Message}. " +
                                                $"Application will close once you hit return.", ConsoleColor.Red);
                Console.WriteLine();
                Console.Write("\t");
                Console.ReadLine();
            }
        }

        public static void ShowHelp()
        {
            ConsoleHelpers.WriteColoredLine("\tOptions:", ConsoleColor.Yellow);

            ConsoleHelpers.WriteColored("\t  -h, --help", ConsoleColor.Green);
            Console.WriteLine("\t\tDisplays this help message.");
            Console.WriteLine();

            ConsoleHelpers.WriteColored("\t  -w, --wallet", ConsoleColor.Green);
            Console.WriteLine("\t\tDisplays your wallet balance on the given exchange.");

            ConsoleHelpers.WriteColored("\t      <exchange>", ConsoleColor.Green);
            Console.WriteLine("\tThe exchange to check, valid options: binance | bin | bittrex | trex.");
            Console.WriteLine();

            ConsoleHelpers.WriteColored("\t  -o, --orders", ConsoleColor.Green);
            Console.WriteLine("\t\tDisplays your open orders on the given exchange.");
            ConsoleHelpers.WriteColored("\t      <exchange>", ConsoleColor.Green);
            Console.WriteLine("\tThe exchange to check, valid options: binance | bin | bittrex | trex.");
            Console.WriteLine();

            ConsoleHelpers.WriteColored("\t  -b, --buy", ConsoleColor.Green);
            Console.WriteLine("\t\tPlaces a buy limit order on the given exchange.");
            ConsoleHelpers.WriteColored("\t      <exchange>", ConsoleColor.Green);
            Console.WriteLine("\tThe exchange to place the order, valid options: binance | bin | bittrex | trex.");
            ConsoleHelpers.WriteColored("\t      <coin>", ConsoleColor.Green);
            Console.WriteLine("\t\tThe coin for which orders should be placed e.g. ETH.");
            ConsoleHelpers.WriteColored("\t      <price>", ConsoleColor.Green);
            Console.WriteLine("\t\tPrice it should be listed for e.g. 0,00031.");
            ConsoleHelpers.WriteColoredLine("\t\t\t\tYou can also enter the above price as 31000.", ConsoleColor.Cyan);
            ConsoleHelpers.WriteColored("\t      <quantity>", ConsoleColor.Green);
            Console.WriteLine("\tThe amount that should be bought.");
            Console.WriteLine();

            ConsoleHelpers.WriteColored("\t  -s, --sell", ConsoleColor.Green);
            Console.WriteLine("\t\tPlaces a sell limit order on the given exchange.");
            ConsoleHelpers.WriteColored("\t      <exchange>", ConsoleColor.Green);
            Console.WriteLine("\tThe exchange to place the order, valid options: binance | bin | bittrex | trex.");
            ConsoleHelpers.WriteColored("\t      <coin>", ConsoleColor.Green);
            Console.WriteLine("\t\tThe coin for which orders should be placed e.g. ETH.");
            ConsoleHelpers.WriteColored("\t      <price>", ConsoleColor.Green);
            Console.WriteLine("\t\tPrice it should be listed for e.g. 0,00031.");
            ConsoleHelpers.WriteColoredLine("\t\t\t\tYou can also enter the above price as 31000.", ConsoleColor.Cyan);
            ConsoleHelpers.WriteColored("\t      <quantity>", ConsoleColor.Green);
            Console.WriteLine("\tThe amount that should be sold.");
            Console.WriteLine();

            ConsoleHelpers.WriteColored("\t  -c, --cancel", ConsoleColor.Green);
            Console.WriteLine("\t\tCancels an order on the given exchange.");
            ConsoleHelpers.WriteColored("\t      <exchange>", ConsoleColor.Green);
            Console.WriteLine("\tThe exchange to check, valid options: binance | bin | bittrex | trex.");
            ConsoleHelpers.WriteColored("\t      <coin>", ConsoleColor.Green);
            Console.WriteLine("\t\tThe coin for which orders should be displayed e.g. ETH.");
            Console.WriteLine();

            ConsoleHelpers.WriteColored("\t  -e, --exit", ConsoleColor.Green);
            Console.WriteLine("\t\tYou're done, time to close the tool.");

            Console.WriteLine();
            ConsoleHelpers.WriteColoredLine("\tSamples:", ConsoleColor.Yellow);
            ConsoleHelpers.WriteColored("\t  -w trex", ConsoleColor.Green);
            Console.WriteLine("\t\t\tShow your wallet balances on Bittrex.");
            ConsoleHelpers.WriteColored("\t  -o trex", ConsoleColor.Green);
            Console.WriteLine("\t\t\tShows all open orders on Bittrex.");
            ConsoleHelpers.WriteColored("\t  -b trex neo 192012 30", ConsoleColor.Green);
            Console.WriteLine("\t\tPlace a buy order for 30 NEO at 0,00192012 on Bittrex.");
            ConsoleHelpers.WriteColored("\t  -s trex eth 0,00932013 15", ConsoleColor.Green);
            Console.WriteLine("\t\tPlace a sell order for 15 ETH at 0,00932013 on Bittrex.");
            ConsoleHelpers.WriteColored("\t  -c trex", ConsoleColor.Green);
            Console.WriteLine("\t\t\tLists all cancellable orders on Bittrex.");
            ConsoleHelpers.WriteColored("\t  -c trex eth", ConsoleColor.Green);
            Console.WriteLine("\t\t\tLists all cancellable ETH orders on Bittrex.");
        }

        private static async Task ShowMenu()
        {
            while (true)
            {
                Console.WriteLine();
                Console.Write("\t> ");

                var result = Console.ReadLine();
                Console.WriteLine();

                if (result != null)
                {
                    const string re = @"\G(""((""""|[^""])+)""|(\S+)) *";
                    var ms = Regex.Matches(result, re);
                    var list = ms.Cast<Match>().Select(m => Regex.Replace(m.Groups[2].Success ? m.Groups[2].Value : m.Groups[4].Value, @"""""", @"""")).ToArray();

                    switch (list[0].ToLower())
                    {
                        case "-h":
                        case "--help":
                            ShowHelp();
                            break;
                        case "-w":
                        case "--wallet":
                            if (list.Length < 2)
                                UnknownCommand();
                            else if (list[1].ToLower() == "trex" || list[1].ToLower() == "bittrex")
                                await GetWallets(ExchangeType.Bittrex);
                            else if (list[1].ToLower() == "bin" || list[1].ToLower() == "binance")
                                await GetWallets(ExchangeType.Binance);
                            else
                                UnknownCommand();
                            break;
                        case "-c":
                        case "--cancel":
                            var coin = list.Length > 2 ? list[2] : null;

                            if (list[1].ToLower() == "trex" || list[1].ToLower() == "bittrex")
                                await CancelOrder(ExchangeType.Bittrex, coin);
                            else if (list[1].ToLower() == "bin" || list[1].ToLower() == "binance")
                                await CancelOrder(ExchangeType.Binance, coin);
                            else
                                UnknownCommand();
                            break;
                        case "-b":
                        case "--buy":
                            if (list.Length < 4)
                                NotEnoughArguments();
                            else
                            {
                                var price = double.Parse(list[3]);
                                var quantity = double.Parse(list[4]);

                                if (list[1].ToLower() == "trex" || list[1].ToLower() == "bittrex")
                                    await Buy(ExchangeType.Bittrex, list[2], price, quantity);
                                else if (list[1].ToLower() == "bin" || list[1].ToLower() == "binance")
                                    await Buy(ExchangeType.Binance, list[2], price, quantity);
                                else
                                    UnknownCommand();
                            }
                            break;
                        case "-s":
                        case "--sell":
                            if (list.Length < 4)
                                NotEnoughArguments();
                            else
                            {
                                var price = double.Parse(list[3]);
                                var quantity = double.Parse(list[4]);

                                if (list[1].ToLower() == "trex" || list[1].ToLower() == "bittrex")
                                    await Sell(ExchangeType.Bittrex, list[2], price, quantity);
                                else if (list[1].ToLower() == "bin" || list[1].ToLower() == "binance")
                                    await Sell(ExchangeType.Binance, list[2], price, quantity);
                                else
                                    UnknownCommand();
                            }
                            break;
                        case "-o":
                        case "--orders":
                            if (list.Length < 2)
                                NotEnoughArguments();
                            else if (list[1].ToLower() == "trex" || list[1].ToLower() == "bittrex")
                                await GetOpenOrders(ExchangeType.Bittrex);
                            else if (list[1].ToLower() == "bin" || list[1].ToLower() == "binance")
                                await GetOpenOrders(ExchangeType.Binance);
                            else
                                UnknownCommand();
                            break;
                        case "-e":
                        case "--exit":
                            Environment.Exit(1);
                            break;
                        default:
                            UnknownCommand();
                            break;
                    }
                }
                else
                {
                    UnknownCommand();
                }
            }
        }

        private static void NotEnoughArguments()
        {
            Console.WriteLine("\tNot enough arguments.");
        }

        private static void UnknownCommand()
        {
            Console.WriteLine("\tUnknown command.");
        }

        public static async Task GetWallets(ExchangeType exchange)
        {
            var prices = await BlockChainApi.Instance.GetPrices();

            if (exchange == ExchangeType.Bittrex)
            {
                await _bittrexManager.GetBalances(prices);
            }
            else if (exchange == ExchangeType.Binance)
            {
                await _binanceManager.GetBalances(prices);
            }
        }

        public static async Task Sell(ExchangeType exchange, string coin, double price, double quantity)
        {
            if (exchange == ExchangeType.Bittrex)
            {
                await _bittrexManager.Sell(coin, price, quantity);
            }
            else if (exchange == ExchangeType.Binance)
            {
                await _binanceManager.Sell(coin, price, quantity);
            }
        }

        public static async Task Buy(ExchangeType exchange, string coin, double price, double quantity)
        {
            if (exchange == ExchangeType.Bittrex)
            {
                await _bittrexManager.Buy(coin, price, quantity);
            }
            else if (exchange == ExchangeType.Binance)
            {
                await _binanceManager.Buy(coin, price, quantity);
            }
        }

        private static async Task GetOpenOrders(ExchangeType exchange)
        {
            var prices = await BlockChainApi.Instance.GetPrices();

            if (exchange == ExchangeType.Bittrex)
            {
                await _bittrexManager.GetOpenOrders(prices);
            }
            else if (exchange == ExchangeType.Binance)
            {
                await _binanceManager.GetOpenOrders(prices);
            }
        }

        private static async Task CancelOrder(ExchangeType exchange, string coin)
        {
            var prices = await BlockChainApi.Instance.GetPrices();

            if (exchange == ExchangeType.Bittrex)
            {
                await _bittrexManager.CancelOrder(prices, coin);
            }
            else if (exchange == ExchangeType.Binance)
            {
                await _binanceManager.CancelOrder(prices, coin);
            }
        }
    }
}

