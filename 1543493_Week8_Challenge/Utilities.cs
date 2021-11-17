using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace _1543493_Week8_Challenge
{
    class Utilities
    {
        public static void PrintShopBanner()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"
(~|_ . |`_|_. _  _   (~ _  _  _| _  |~) _ _  _  _  _
_)| ||~|~ | || |(_|  _)(_|| |(_|_\  |_)(_|/_(_|(_|| 
                 _|                                 ");
            Console.ResetColor();
        }

        public static string GetUserInput(string text)
        {
            Console.WriteLine(text);

            return Console.ReadLine().ToLower().Trim();
        }

        public static bool InputVerification(string userInput)
        {
            while (true)
            {
                Console.WriteLine(userInput);

                string inputReply = Console.ReadLine().ToLower().Trim();

                switch (inputReply)
                {
                    case "yes":
                    case "y":
                        {
                            return true;
                        }
                    case "no":
                    case "n":
                        {
                            return false;
                        }
                }
            }
        }

        public static string DecimalToString(decimal decimalToConvert)
        {
            string cultureCurrency = "C";
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

            string formatted = decimalToConvert.ToString(cultureCurrency, culture);

            return formatted;
        }

        public static decimal StringToDecimal(string decimalString)
        {
            if (!decimal.TryParse(decimalString, out decimal newDecimal))
            {
                Console.WriteLine("\nUnable to parse as decimal.");
                return 0;
            }

            return newDecimal;
        }

        public static int StringToInt(string intString)
        {
            if (!int.TryParse(intString, out int newInt))
            {
                Console.WriteLine("\nUnable to parse as integer.");
                return 0;
            }

            return newInt;
        }
    }
}
