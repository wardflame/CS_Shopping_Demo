using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace _1543493_Week8_Challenge
{
    class Utilities
    {
        /// <summary>
        /// Print the Shifting Sands Bazaar banner.
        /// </summary>
        public static void PrintShopBanner()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"
(~|_ . |`_|_. _  _   (~ _  _  _| _  |~) _ _  _  _  _
_)| ||~|~ | || |(_|  _)(_|| |(_|_\  |_)(_|/_(_|(_|| 
                 _|                                 ");
            Console.ResetColor();
        }

        /// <summary>
        /// Get user input in string with prompt so as to reduce WriteLine()s required.
        /// </summary>
        /// <param name="text"></text to prompt user input.>
        /// <returns></user input.>
        public static string GetUserInput(string text)
        {
            Console.WriteLine(text);

            return Console.ReadLine().ToLower().Trim();
        }

        /// <summary>
        /// Generic method to ask whether a situation was correct or incorrect. Parse in
        /// the question to be repeat in the event of an invalid response.
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Take a decimal value and return it as a string, formatted as currency, USD.
        /// </summary>
        /// <param name="decimalToConvert"></param>
        /// <returns></decimal as string USD.>
        public static string DecimalToString(decimal decimalToConvert)
        {
            string cultureCurrency = "C";
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

            string formatted = decimalToConvert.ToString(cultureCurrency, culture);

            return formatted;
        }

        /// <summary>
        /// Turn a valid string into a decimal.
        /// </summary>
        /// <param name="decimalString"></param>
        /// <returns></string as decimal.>
        public static decimal StringToDecimal(string decimalString)
        {
            if (!decimal.TryParse(decimalString, out decimal newDecimal))
            {
                StringWithColor("\nUnable to parse as decimal.", ConsoleColor.Red, false);
                return 0;
            }

            return newDecimal;
        }

        /// <summary>
        /// Turn a valid string into an integer.
        /// </summary>
        /// <param name="intString"></param>
        /// <returns></string as integer.>
        public static int StringToInt(string intString)
        {
            if (!int.TryParse(intString, out int newInt))
            {
                StringWithColor("\nUnable to parse as integer.", ConsoleColor.Red, false);
                return 0;
            }

            return newInt;
        }

        /// <summary>
        /// Parse in a string and process with colour. Provide boolean to see if the string
        /// should be a Write() (true) or a WriteLine() (false).
        /// </summary>
        /// <param name="input"></string to be processed with colour.>
        /// <param name="color"></desired colour for string.>
        /// <param name="writeOrLine"></whether the string should be a Write(true) or WriteLine(false)>
        public static void StringWithColor(string input, ConsoleColor color, bool writeOrLine)
        {
            Console.ForegroundColor = color;
            if (writeOrLine)
            {
                Console.Write(input);
            }
            else
            {
                Console.WriteLine(input);
            }
            Console.ResetColor();
        }
    }
}
