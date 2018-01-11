using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTools.QuickExchange
{
    public static class ConsoleHelpers
    {
        public static void WriteSeparator()
        {
            Console.WriteLine();
            Console.WriteLine("==================================================================================================");
            Console.WriteLine();
        }

        public static void WriteDonation()
        {
            Console.WriteLine();
            Console.WriteLine("\t Like our tools? Please consider a donation of whatever they're worth to you.");
            Console.WriteLine();
            Console.WriteLine("\t BTC - 12QZY5azAv3wV2hFLAvqPPXNvEDYAn3Z4U");
            Console.WriteLine("\t ETH - 0x4398c958468bEDB41DdEF4C297eB543c6d26f440");
            Console.WriteLine("\t LTC - LeQFDs3oiVP6Kx86miWndZp4WxjLGEbc4W");
            Console.WriteLine();
        }

        public static void WriteIntro()
        {
            Console.WriteLine("");

            Console.WriteLine("\t " + @"   ____       _     __    ____         __                      ");
            Console.WriteLine("\t " + @"  / __ \__ __(_)___/ /__ / __/_ ______/ /  ___ ____  ___ ____  ");
            Console.WriteLine("\t " + @" / /_/ / // / / __/  '_// _/ \ \ / __/ _ \/ _ `/ _ \/ _ `/ -_) ");
            Console.WriteLine("\t " + @" \___\_\_,_/_/\__/_/\_\/___//_\_\\__/_//_/\_,_/_//_/\_, /\__/  ");
            Console.WriteLine("\t " + @"                                                   /___/       ");
            Console.WriteLine("\t      made by CryptoTools              ");

            Console.WriteLine("");
        }

        public static void WriteMenu()
        {
            Console.WriteLine("\t1. Check my open orders");
            Console.WriteLine("\t2. Check my wallet");
            Console.WriteLine("\t3. Place a buy order");
            Console.WriteLine("\t4. Place a sell order");
            Console.WriteLine("\t5. Cancel an order");
            Console.WriteLine();
            Console.WriteLine("\t6. Close the tool");

            Console.WriteLine();
            Console.Write("\tWhat do you want to do? ");
        }

        public static void WriteColoredLine(string line, ConsoleColor color, bool padded = false)
        {
            Console.ForegroundColor = color;
            if (padded) Console.WriteLine();
            Console.WriteLine(line);
            if (padded) Console.WriteLine();
            Console.ResetColor();
        }

        public static void WriteColored(string line, ConsoleColor color, bool padded = false)
        {
            Console.ForegroundColor = color;
            if (padded) Console.WriteLine();
            Console.Write(line);
            if (padded) Console.WriteLine();
            Console.ResetColor();
        }

        public static string ToStringTable<T>(
          this IEnumerable<T> values,
          string[] columnHeaders,
          params Func<T, object>[] valueSelectors)
        {
            return ToStringTable(values.ToArray(), columnHeaders, valueSelectors);
        }

        public static string ToStringTable<T>(
          this T[] values,
          string[] columnHeaders,
          params Func<T, object>[] valueSelectors)
        {
            Debug.Assert(columnHeaders.Length == valueSelectors.Length);

            var arrValues = new string[values.Length + 1, valueSelectors.Length];

            // Fill headers
            for (int colIndex = 0; colIndex < arrValues.GetLength(1); colIndex++)
            {
                arrValues[0, colIndex] = columnHeaders[colIndex];
            }

            // Fill table rows
            for (int rowIndex = 1; rowIndex < arrValues.GetLength(0); rowIndex++)
            {
                for (int colIndex = 0; colIndex < arrValues.GetLength(1); colIndex++)
                {
                    arrValues[rowIndex, colIndex] = valueSelectors[colIndex]
                      .Invoke(values[rowIndex - 1]).ToString();
                }
            }

            return ToStringTable(arrValues);
        }

        public static string ToStringTable(this string[,] arrValues)
        {
            int[] maxColumnsWidth = GetMaxColumnsWidth(arrValues);
            var headerSpliter = new string('-', maxColumnsWidth.Sum(i => i + 3) - 1);

            var sb = new StringBuilder();
            for (int rowIndex = 0; rowIndex < arrValues.GetLength(0); rowIndex++)
            {
                for (int colIndex = 0; colIndex < arrValues.GetLength(1); colIndex++)
                {
                    // Print cell
                    string cell = arrValues[rowIndex, colIndex];
                    cell = cell.PadRight(maxColumnsWidth[colIndex]);

                    if (colIndex == 0)
                        sb.Append("\t");

                    sb.Append(" | ");
                    sb.Append(cell);
                }

                // Print end of line
                sb.Append(" | ");
                sb.AppendLine();

                // Print splitter
                if (rowIndex == 0)
                {
                    sb.AppendFormat("\t |{0}| ", headerSpliter);
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        private static int[] GetMaxColumnsWidth(string[,] arrValues)
        {
            var maxColumnsWidth = new int[arrValues.GetLength(1)];
            for (int colIndex = 0; colIndex < arrValues.GetLength(1); colIndex++)
            {
                for (int rowIndex = 0; rowIndex < arrValues.GetLength(0); rowIndex++)
                {
                    int newLength = arrValues[rowIndex, colIndex].Length;
                    int oldLength = maxColumnsWidth[colIndex];

                    if (newLength > oldLength)
                    {
                        maxColumnsWidth[colIndex] = newLength;
                    }
                }
            }

            return maxColumnsWidth;
        }
    }
}
