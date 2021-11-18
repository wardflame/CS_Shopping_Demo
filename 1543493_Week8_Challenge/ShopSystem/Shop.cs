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
    public class Shop
    {
        public static bool running = true;

        public SaveData saveData = new SaveData();

        public Client client;
        public List<Client> clientList = new List<Client>();

        public List<Item> bazaarInventory = new List<Item>();

        public static void InShop()
        {
            Shop shop = InitShop();

            while (running)
            {
                string shopBranch = shop.ShopMainMenu();
                switch (shopBranch)
                {
                    case "1":
                        {
                            shop.ClientPurchaseArms();
                        }
                        break;
                    default:
                        {
                            Utilities.StringWithColor("\nInput a valid option.", ConsoleColor.Red, false);
                            Thread.Sleep(1500);
                            Console.Clear();
                        }
                        break;
                }
            }
        }

        private static Shop InitShop()
        {
            Shop shop = new Shop(); ;
            if (Directory.Exists("Saves"))
            {

            }
            else
            {
                Directory.CreateDirectory("Saves");
            }

            if (File.Exists(@"Saves\save.json"))
            {
                shop.saveData = SaveData.LoadShop();
                shop.client = shop.saveData.currentClient;
                shop.clientList = shop.saveData.clientList;
                shop.bazaarInventory = shop.saveData.bazaarInventory;
            }
            else
            {
                Utilities.PrintShopBanner();
                shop.bazaarInventory = InitShopInventory();
                shop.client = Client.CreateNewClient();
                shop.clientList.Add(shop.client);
                shop.saveData.currentClient = shop.client;
                shop.saveData.clientList = shop.clientList;
                shop.saveData.bazaarInventory = shop.bazaarInventory;
                shop.saveData.SaveShop();
            }
            return shop;
        }

        private static List<Item> InitShopInventory()
        {
            List<Item> list = new List<Item>();

            Item ak47 = new Item("AK-47", "Reliable, 7.62x39mm automatic rifle, unrivalled in its power under environmental stress.", Item.ItemType.SmallArms, 759.95m)
            {
                stock = 2000
            };

            Item pkm = new Item("PKM", "Unrelenting, 7.62x54mmR machine gun.", Item.ItemType.SmallArms, 995.99m)
            {
                stock = 800
            };

            Item rpg7 = new Item("RPG-7", "Unguided, handheld, reusable rocket launcher.", Item.ItemType.SmallArms, 1250.99m)
            {
                stock = 1200
            };

            Item toyotaHilux = new Item("Toyota Hilux", "A robust, no-nonsense all-terrain pickup truck, ready for HMG attachments.", Item.ItemType.GroundVehicles, 46885m)
            {
                stock = 100
            };

            Item t34 = new Item("T-34/76", "An old, inexpensive Soviet tank mass-produced during the second world war.", Item.ItemType.GroundVehicles, 230000m)
            {
                stock = 50
            };

            Item abrams = new Item("M1 Abrams", "The top of the line from Shifting Sands. Civil wars can be fought with only a handful of cutting-edge battle tanks.", Item.ItemType.GroundVehicles, 6200000m)
            {
                stock = 8
            };

            Item raven = new Item("R-44 Raven II", "A standard civilian helicopter, capable of carrying a small fireteam and some equipment.", Item.ItemType.AerialVehicles, 465000m)
            {
                stock = 20
            };

            Item mh6 = new Item("MH-6 Little Bird", "A small, agile helicopter capable of housing machine guns and rockets. Small bird, big caw.", Item.ItemType.AerialVehicles, 2000000m)
            {
                stock = 10
            };

            Item apache = new Item("AH-64 Apache", "The apex predator of the sky. Comes with thermal imaging, explosive machine guns and missiles. Fear the skies.", Item.ItemType.AerialVehicles, 930000000m)
            {
                stock = 5
            };

            list.Add(ak47);
            list.Add(pkm);
            list.Add(rpg7);
            list.Add(toyotaHilux);
            list.Add(t34);
            list.Add(abrams);
            list.Add(raven);
            list.Add(mh6);
            list.Add(apache);

            return list;
        }

        private string ShopMainMenu()
        {
            Utilities.PrintShopBanner();
            Console.WriteLine($"\nWelcome to the Shifting Sands Bazaar. We wish you a pleasant experience whilst browsing our armaments.");

            client.PrintClientBalance();

            string input = Utilities.GetUserInput("\n1. Buy armaments" +
                "\n2. Sell armaments" +
                "\n3. Review/Edit basket" +
                "\n4. Review client's inventory");

            return input;
        }

        private void ClientPurchaseArms()
        {
            Console.Clear();
            bool browsing = true;
            while (browsing)
            {
                Utilities.PrintShopBanner();
                int iSize = PrintShopInventory();
                client.PrintClientBalance();
                bool canAfford = PrintBasket();

                Item desired;
                while (true)
                {
                    string input = Utilities.GetUserInput($"\n1. Select an item by number to add to basket" +
                        $"\n2. Press e to edit basket" +
                        $"\n3. Press c to checkout basket" +
                        $"\n4. Input x to return to main menu");

                    if (int.TryParse(input, out int inputInt) && inputInt > 0 && inputInt <= iSize)
                    {
                        desired = GetItemFromShop(bazaarInventory, inputInt);
                        AddItemToBasket(desired);
                        Thread.Sleep(2000);
                        break;
                    }
                    else if (input == "c")
                    {
                        if (canAfford)
                        {

                        }
                        else
                        {
                            Utilities.StringWithColor($"\nInsufficient client funds. Unable to proceed.", ConsoleColor.DarkRed, false);
                            Thread.Sleep(2000);
                            break;
                        }
                    }
                    else if (input == "x")
                    {
                        Utilities.StringWithColor("\nReturning to main menu...", ConsoleColor.Yellow, false);
                        browsing = false;
                        Thread.Sleep(1500);
                        break;
                    }
                    else
                    {
                        Utilities.StringWithColor("\nInvalid response.", ConsoleColor.Red, false);
                        Thread.Sleep(1500);
                        break;
                    }
                }
                Console.Clear();
            }
        }

        private void ClientSellArms()
        {

        }

        private void ClientReviewBasket()
        {

        }

        private void ClientReviewInventory()
        {

        }

        private int PrintShopInventory()
        {
            bazaarInventory.Sort((itemA, itemB) => itemA.itemType.CompareTo(itemB.itemType));

            string[] categories = new string[] { "\n>> Small Arms <<", "\n>> Ground Vehicles <<", "\n>> Aerial Vehicles <<" };
            Item.ItemType previousItemType = 0;

            int inventorySize = 0;
            for (int i = 0; i < bazaarInventory.Count; i++)
            {
                Item item = bazaarInventory[i];

                if (previousItemType != item.itemType || i == 0)
                {
                    Utilities.StringWithColor(categories[(int)item.itemType], ConsoleColor.DarkGreen, false);
                }

                Console.WriteLine($"\n{i + 1}. {item}");
                previousItemType = item.itemType;

                inventorySize += i;
            }

            return inventorySize;
        }

        private Item GetItemFromShop(List<Item> list, int inputInt)
        {
            Item selectedItem = null;

            while (selectedItem == null)
            {
                if (inputInt > 0 && inputInt <= list.Count)
                {
                    selectedItem = list[inputInt - 1];
                }
                else
                {
                    Console.WriteLine("\nPlease try again.\n");
                }
            }

            Console.WriteLine($"\n{selectedItem.name} selected!");
            return selectedItem;
        }

        private void AddItemToBasket(Item item)
        {
            while (true)
            {
                string input = Utilities.GetUserInput($"\nHow many {item.name}s would you like to add to your basket? (type x to exit to previous menu)");
                if (input == "x")
                {
                    break;
                }

                int amount = Utilities.StringToInt(input);
                if (StockCheck(item, amount))
                {
                    Item basketItem = item.Clone();
                    basketItem.stock += amount;
                    item.stock -= amount;

                    client.basket.Add(basketItem);

                    Utilities.StringWithColor($"\n{basketItem.name} added to basket.", ConsoleColor.DarkGreen, false);
                    
                    break;
                }
            }
        }

        private bool PrintBasket()
        {
            Utilities.StringWithColor($"\n>> Current basket <<", ConsoleColor.Cyan, false);

            if (client.basket.Count == 0)
            {
                Utilities.StringWithColor("\nNo items currently in basket.", ConsoleColor.DarkGray, false);
            }

            decimal sumBasket = 0;
            foreach (Item items in client.basket)
            {
                sumBasket += items.stock * items.cost;
                Console.WriteLine($"\n{items.name}\nQuantity in basket: {items.stock}\nCost (total quantity): {Utilities.DecimalToString(items.cost * items.stock)}");
            }

            if (sumBasket > client.balance)
            {
                Utilities.StringWithColor($"\nBasket total: ", ConsoleColor.DarkGray, true);
                Utilities.StringWithColor($"{Utilities.DecimalToString(sumBasket)}", ConsoleColor.DarkRed, true);
                Utilities.StringWithColor($" exceeds client balance: ", ConsoleColor.DarkGray, true);
                Utilities.StringWithColor($"{Utilities.DecimalToString(client.balance)}", ConsoleColor.Yellow, false);
                Utilities.StringWithColor($"Please edit basket until total comes withing range of client balance.", ConsoleColor.DarkGray, false);
                return false;
            }
            Console.WriteLine($"\nBasket total: {Utilities.DecimalToString(sumBasket)}");
            return true;
        }

        private bool StockCheck(Item item, int deduction)
        {
            if (item.stock < deduction)
            {
                Utilities.StringWithColor("\nNot enough stock to add to basket. Please reduce the required quantity.", ConsoleColor.Red, false);
                return false;
            }
            return true;
        }

        private void CheckoutBasket()
        {

        }
    }
}
