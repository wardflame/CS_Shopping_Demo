using _1543493_Week8_Challenge.ClientContent;
using _1543493_Week8_Challenge.ItemContent;
using _1543493_Week8_Challenge.SaveSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace _1543493_Week8_Challenge.ShopSystem
{
    class Shop
    {
        public static bool running = true;

        public static List<Client> clientList = new List<Client>();
        public static Client client;

        public static List<Item> bazaarInventory = new List<Item>();

        public static SaveData saveData = new SaveData(); 

        public void InShop()
        {
            if (File.Exists("save.json"))
            {
                saveData = SaveData.LoadShop();
                client = saveData.currentClient;
                clientList = saveData.clientList;
                bazaarInventory = saveData.bazaarInventory;
            }
            else
            {

            }

            while (running)
            {
                Utilities.PrintShopBanner();

                Console.WriteLine($"\nWelcome to the Shifting Sands Bazaar. We wish you a pleasant experience whilst browsing our armaments.");

                if (clientList.Count <= 0)
                {
                    client = Client.InitClient(client, clientList);
                    saveData.currentClient = client;
                    saveData.clientList = clientList;
                    saveData.SaveShop();
                }
                else
                {
                    client = Client.ChooseClient(client, clientList);
                }

                string shopBranch = ShopMainMenu();

                switch (shopBranch)
                {
                    case "1":
                        {

                        }
                        break;
                }
            }
        }

        private static void InitShopInventory()
        {
            Item AK47 = new Item("AK-47", "Happy Hunting!", Item.ItemType.SmallArms, 759.95m)
            {
                stock = 150
            };

            Item RPG7 = new Item("RPG-7", "'RPG, Taliban pffm.", Item.ItemType.SmallArms, 1250.99m)
            {
                stock = 70
            };

            Item PKM = new Item("PKM", "Mowin' 'em down.", Item.ItemType.SmallArms, 995.99m)
            {
                stock = 40
            };
        }

        private static string ShopMainMenu()
        {
            client.PrintClientBalance();

            string input = Utilities.GetUserInput("\n____ Main Menu ____\n" +
                "\n1. Buy armaments" +
                "\n2. Sell armaments" +
                "\n3. Review/Edit basket" +
                "\n4. Review personal inventory");

            return input;
        }
    }
}
