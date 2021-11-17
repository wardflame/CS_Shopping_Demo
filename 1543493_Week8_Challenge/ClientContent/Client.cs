using _1543493_Week8_Challenge.ItemContent;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace _1543493_Week8_Challenge.ClientContent
{
    public class Client
    {
        public string name;
        public decimal balance;
        public List<Item> inventory = new List<Item>();
        public List<Item> basket = new List<Item>();

        public override string ToString()
        {
            return name;
        }

        public static Client CreateNewClient()
        {
            Client client = new Client();

            client.name = GetClientName();
            Thread.Sleep(2000);
            Console.Clear();
            Utilities.PrintShopBanner();

            client.balance = GetClientBalance(client.name);
            Thread.Sleep(2000);
            Console.Clear();
            Utilities.PrintShopBanner();

            return client;
        }

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

        private static decimal GetClientBalance(string clientName)
        {
            Console.Write($"\nInput {clientName}'s balance: ");
            string input = Console.ReadLine();

            if (!decimal.TryParse(input, out _))
            {
                Console.WriteLine("\nInvalid response. Please input a number.");
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

        public static Client InitClient(Client client, List<Client> clientList)
        {
            client = CreateNewClient();
            return client;
        }

        public static Client ChooseClient(Client client, List<Client> clientList)
        {
            Console.WriteLine(">> Client List <<");

            for (int i = 0; i < clientList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {clientList[i]}");
            }

            string input = Utilities.GetUserInput("\nPlease select the client to shop for. (#)");
            int inputNum = Utilities.StringToInt(input) - 1;

            if (inputNum < 0 || inputNum > clientList.Count)
            {
                Console.WriteLine("\nInput a valid response.");
                Thread.Sleep(2000);
                Console.Clear();
                Utilities.PrintShopBanner();
                ChooseClient(client, clientList);
            }

            if (!Utilities.InputVerification($"\nAre you sure you wish to shop for {clientList[inputNum]}? (y/n)"))
            {
                Console.Clear();
                ChooseClient(client, clientList);
            }

            client = clientList[inputNum];
            return client;
        }

        public void PrintClientBalance()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (name.EndsWith("s"))
            {
                Console.Write($"\n{name}' balance");
            }
            else
            {
                Console.Write($"\n{name}'s balance");
            }
            Console.ResetColor();
            Console.WriteLine($": {Utilities.DecimalToString(balance)}");
        }
    }
}
