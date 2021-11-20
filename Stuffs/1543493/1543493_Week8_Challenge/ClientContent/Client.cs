using _1543493_Week8_Challenge.ItemContent;
using _1543493_Week8_Challenge.ShopSystem;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace _1543493_Week8_Challenge.ClientContent
{
    public class Client
    {
        public string name;
        public decimal balance; // PERMITTED TO USE DECIMAL BY VIKTOR AS FLOAT CAUSED ROUNDING ERRORS WITH HIGH-VALUE ITEMS.
        public List<Item> basket = new List<Item>(); // BRIEF REQUESTED BASKET IN SHOP, HOWEVER, WITH SAVING, I DEEM IT MORE UX FRIENDLY TO HAVE THE BASKET SAVE, SO THAT THE CLIENT CAN RETURN TO IT.
        public List<Item> inventory = new List<Item>();

        public override string ToString()
        {
            return name;
        }

        /// <summary>
        /// A client creator, allowing the designation of a client name and balance/budget.
        /// </summary>
        /// <returns></a new client.>
        public static Client CreateNewClient()
        {
            Client client = new Client();

            client.name = GetClientName();
            client.balance = GetClientBalance(client.name);

            return client;
        }

        /// <summary>
        /// Ask user for client's name. Start again if user decides input isn't client name.
        /// </summary>
        /// <returns></string input for client name.>
        private static string GetClientName()
        {
            Console.Write("\nInput new client's name: ");
            string input = Console.ReadLine();

            if (!Utilities.InputVerification($"\nAre you sure {input} is the new client's name? (y/n)"))
            {
                return GetClientName();
            }

            Console.WriteLine($"\nNew client: {input}");

            return input;
        }

        /// <summary>
        /// Ask user for client's budget/balance-convert input to decimal. If input isn't parsable as decimal, throw an error and return 0 value.
        /// </summary>
        /// <param name="clientName"></Parse client name in for clarity.>
        /// <returns></decimal for client budget/balance.>
        private static decimal GetClientBalance(string clientName)
        {
            Console.Write($"\nInput {clientName}'s balance: ");
            string input = Console.ReadLine();

            if (!decimal.TryParse(input, out _))
            {
                Console.WriteLine("\nInvalid response.");
                Thread.Sleep(2000);
                Console.Clear();
                Utilities.PrintShopBanner();
                return GetClientBalance(clientName);
            }

            decimal balance = Utilities.StringToDecimal(input);
            string balanceString = Utilities.DecimalToString(balance);

            if (!Utilities.InputVerification($"\nAre you sure {clientName}'s balance is {balanceString}? (y/n)"))
            {
                return GetClientBalance(clientName);
            }

            decimal.Round(balance, 2);

            Console.WriteLine($"\n{clientName}'s balance: {balanceString}");

            return balance;
        }

        /// <summary>
        /// Print a list of users from a shop object client list. User chooses client to browse as, or to create a new client.
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></client to overwrite existing in shop (switch clients)>
        public static Client ChooseClient(Shop shop)
        {
            Utilities.PrintShopBanner();
            Client chosenClient;

            Utilities.StringWithColor("\n>> Client List <<\n", ConsoleColor.Cyan, false);

            for (int i = 0; i < shop.clientList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {shop.clientList[i]}");
            }

            string input = Utilities.GetUserInput("\n1. Select the client to shop for by number" +
                "\n2. Input c to create a new client");

            while (true)
            {
                if (input == "c")
                {
                    chosenClient = CreateNewClient();
                    shop.clientList.Add(chosenClient);
                    shop.saveData.clientList = shop.clientList;
                    shop.saveData.SaveShop();
                    break;
                }
                else if (int.TryParse(input, out int inputNum) && inputNum <= shop.clientList.Count && inputNum > 0)
                {
                    chosenClient = shop.clientList[inputNum - 1];
                    break;
                }
                else
                {
                    Utilities.StringWithColor("\nInvalid input.", ConsoleColor.Red, false);
                    Thread.Sleep(1500);
                    Console.Clear();
                    return ChooseClient(shop);
                }
            }

            if (!Utilities.InputVerification($"\nAre you sure you wish to shop for {chosenClient.name}? (y/n)"))
            {
                Console.Clear();
                return ChooseClient(shop);
            }
            return chosenClient;
        }

        /// <summary>
        /// Get client's current balance and provide correct apostrophe depending on name's last character.
        /// </summary>
        public void PrintClientBalance()
        {
            if (name.EndsWith("s"))
            {
                Utilities.StringWithColor($"\n{name}' balance", ConsoleColor.Yellow, true);
            }
            else
            {
                Utilities.StringWithColor($"\n{name}'s balance", ConsoleColor.Yellow, true);
            }
            Console.WriteLine($": {Utilities.DecimalToString(balance)}");
        }
    }
}
