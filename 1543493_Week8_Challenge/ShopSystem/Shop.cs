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
        public static decimal sellbackRate = 0.75m;

        public SaveData saveData = new SaveData();

        public Client client;
        public List<Client> clientList = new List<Client>();

        public List<Item> bazaarInventory = new List<Item>();

        /// <summary>
        /// Central hub for shopping. User provides input in the main menu to branch out to different parts of the shop.
        /// Check for save file. If save is available, load information. If there are clients available in the client list,
        /// have user choose a client from the list. In future, I would add a password system for client security.
        /// </summary>
        public static void InShop()
        {
            Shop shop = InitShop();

            while (running)
            {
                if (shop.clientList.Count > 0)
                {
                    shop.client = Client.ChooseClient(shop);
                    shop.saveData.currentClient = shop.client;
                    shop.saveData.SaveShop();
                    Console.Clear();
                }

                bool shopping = true;
                while (shopping)
                {
                    string shopBranch = shop.ShopMainMenu();
                    switch (shopBranch)
                    {
                        case "1":
                            {
                                shop.ClientPurchaseArms();
                            }
                            break;
                        case "2":
                            {
                                shop.ClientReviewSellInventory();
                            }
                            break;
                        case "3":
                            {
                                Console.Clear();
                                shop.client = Client.ChooseClient(shop);
                                shop.saveData.currentClient = shop.client;
                                shop.saveData.SaveShop();
                                Console.Clear();
                            }
                            break;
                        case "x":
                            {
                                Environment.Exit(1);
                            }
                            break;
                        default:
                            {
                                Utilities.StringWithColor("\nInvalid input.", ConsoleColor.Red, false);
                                Thread.Sleep(1500);
                                Console.Clear();
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Is there a Saves directory? If not, make it. Does a save.json exist there? If not, create a first client and make
        /// a new save.json in Saves.
        /// </summary>
        /// <returns></return a shop object with all information needed to start.>
        private static Shop InitShop()
        {
            Shop shop = new Shop(); ;
            if (!Directory.Exists("Saves"))
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
                Console.Clear();
            }
            return shop;
        }

        /// <summary>
        /// Create items for the shop inventory, with appropriate stocks.
        /// </summary>
        /// <returns></return the new list with all items.>
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

        /// <summary>
        /// Print a menu for the user to navigate the different areas of the shop.
        /// </summary>
        /// <returns></user input for calling methods.>
        private string ShopMainMenu()
        {
            Utilities.PrintShopBanner();
            Console.WriteLine($"\nWelcome to the Shifting Sands Bazaar. We wish you a pleasant experience whilst browsing our armaments.");

            client.PrintClientBalance();

            string input = Utilities.GetUserInput("\n1. Buy armaments" +
                "\n2. Review/Sell client's inventory" +
                "\n3. Choose different client" +
                "\n4. Input x to exit application");

            return input;
        }

        /// <summary>
        /// Print the current, sorted shop inventory for the user to browse. Input a number correlating to select it and then
        /// input a second number for how many of the item the user wants to add to the inventory. Adding items to basket reduces
        /// the shop's stock whilst it's in there. User can edit their basket or checkout items from this menu. If the user lacks
        /// the funds to go to checkout, notification is given, and the checkout menu is inaccessible.
        /// </summary>
        private void ClientPurchaseArms()
        {
            Console.Clear();
            bool browsing = true;
            while (browsing)
            {
                Utilities.PrintShopBanner();
                int iSize = PrintShopInventory();
                decimal totalBasket = PrintBasket();
                client.PrintClientBalance();

                Item desired;
                while (true)
                {
                    string input = Utilities.GetUserInput($"\n1. Select an item by number to add to basket" +
                        $"\n2. Press e to edit basket" +
                        $"\n3. Press c to checkout basket" +
                        $"\n4. Input x to return to main menu");

                    if (int.TryParse(input, out int inputInt) && inputInt > 0 && inputInt <= iSize)
                    {
                        desired = GetItemFromShop(inputInt);
                        if (desired != null)
                        {
                            AddItemToBasket(desired);
                        }
                        Thread.Sleep(2000);
                        break;
                    }
                    else if (input == "e")
                    {
                        ClientEditBasket();
                        break;
                    }
                    else if (input == "c")
                    {
                        if (totalBasket < client.balance)
                        {
                            CheckoutBasket();
                            break;
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
                        browsing = false;
                        break;
                    }
                    else
                    {
                        Utilities.StringWithColor("\nInvalid input.", ConsoleColor.Red, false);
                        Thread.Sleep(1500);
                        break;
                    }
                }
                Console.Clear();
            }
        }


        /// <summary>
        /// User can navigate the client's basket, similarly to the ClientPurchaseArms method, allowing them to select an item
        /// in the basket and remove an input quantity of the item. Basket stock returns to the shop stock when removed.
        /// </summary>
        private void ClientEditBasket()
        {
            Console.Clear();
            bool reviewingBasket = true;
            while (reviewingBasket)
            {
                Utilities.PrintShopBanner();
                PrintBasket();

                Item desired;
                while (true)
                {
                    string input = Utilities.GetUserInput($"\n1. Select an item by number to remove from basket" +
                        $"\n2. Input x to return to previous menu");

                    if (int.TryParse(input, out int inputInt) && inputInt > 0 && inputInt <= client.basket.Count)
                    {
                        desired = GetItemFromBasket(inputInt);
                        if (desired != null)
                        {
                            RemoveItemFromBasket(desired);
                        }
                        Thread.Sleep(2000);
                        break;
                    }
                    else if (input == "x")
                    {
                        reviewingBasket = false;
                        break;
                    }
                    else
                    {
                        Utilities.StringWithColor("\nInvalid input.", ConsoleColor.Red, false);
                        Thread.Sleep(1500);
                        break;
                    }
                }
                Console.Clear();
            }
        }

        /// <summary>
        /// User reviews the client's purchased inventory, stored separately. After purchasing items from the shop, goods are
        /// added to this inventory list. Goods can be selected and sold back to the shop at 75% of original price.
        /// </summary>
        private void ClientReviewSellInventory()
        {
            Console.Clear();
            bool reviewSell = true;
            while (reviewSell)
            {
                int iSize = PrintClientInventory();
                client.PrintClientBalance();

                Item desired;
                while (true)
                {
                    string input = Utilities.GetUserInput($"\n1. Choose an item by number to sell back to the bazaar" +
                    $"\n2. Input x to return to previous menu");

                    if (int.TryParse(input, out int inputInt) && inputInt > 0 && inputInt <= iSize)
                    {
                        desired = GetItemFromInventory(inputInt);
                        if (desired != null)
                        {
                            SellItemToShop(desired);
                        }
                        Thread.Sleep(2000);
                        break;
                    }
                    else if (input == "x")
                    {
                        reviewSell = false;
                        break;
                    }
                    else
                    {
                        Utilities.StringWithColor("\nInvalid input.", ConsoleColor.Red, false);
                        Thread.Sleep(1500);
                        break;
                    }
                }
                Console.Clear();
            }
        }

        /// <summary>
        /// Read the parsed integer and see if it is within range of the client's inventory. If it is, get the item in the inventory
        /// at the integer's index.
        /// </summary>
        /// <param name="inputInt"></parsed to acquire item at index int.>
        /// <returns></returns item at the inputInt index.>
        private Item GetItemFromInventory(int inputInt)
        {
            Item selectedItem = null;

            while (selectedItem == null)
            {
                if (inputInt > 0 && inputInt <= client.inventory.Count)
                {
                    selectedItem = client.inventory[inputInt - 1];
                    Console.WriteLine($"\n{selectedItem.name} selected!");
                }
                else
                {
                    Console.WriteLine("\nPlease try again.\n");
                }
            }
            return selectedItem;
        }

        /// <summary>
        /// Sort the client's inventory by enum type and then print them out, each headered by their type from a string array.
        /// Provide the total cost of each item with the reduced market rate for selling back. Also provide a grand total cost for
        /// all items in inventory, reduced too.
        /// </summary>
        /// <returns></returns>
        private int PrintClientInventory()
        {
            Utilities.StringWithColor($"\n>> Inventory: {client.name} <<", ConsoleColor.Cyan, false);

            if (client.inventory.Count == 0)
            {
                Utilities.StringWithColor("\nNo items currently in inventory.", ConsoleColor.DarkGray, false);
            }

            client.inventory.Sort((itemA, itemB) => itemA.itemType.CompareTo(itemB.itemType));

            string[] categories = new string[] { "\n>> Small Arms <<", "\n>> Ground Vehicles <<", "\n>> Aerial Vehicles <<" };
            Item.ItemType previousItemType = 0;

            int inventorySize = 0;
            decimal sumInventory = 0;
            for (int i = 0; i < client.inventory.Count; i++)
            {
                Item item = client.inventory[i];

                if (previousItemType != item.itemType || i == 0)
                {
                    Utilities.StringWithColor(categories[(int)item.itemType], ConsoleColor.DarkGreen, false);
                }

                Console.WriteLine($"\n{i + 1}. {item.name}\nInventory stock: {item.stock}\nCost (total stock /w sell back rate): {Utilities.DecimalToString((item.cost * item.stock) * sellbackRate)}");
                previousItemType = item.itemType;
                inventorySize += 1;
                sumInventory = item.cost * item.stock;
            }
            Console.WriteLine($"\nInventory total: {Utilities.DecimalToString(sumInventory * sellbackRate)}");

            return inventorySize;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

                if (item.stock == 0)
                {
                    Utilities.StringWithColor($"\n{i + 1}. {item.name} [OUT OF STOCK]", ConsoleColor.DarkRed, false);
                }
                else
                {
                    Console.WriteLine($"\n{i + 1}. {item}");
                }
                previousItemType = item.itemType;

                inventorySize += 1;
            }

            return inventorySize;
        }

        private void SellItemToShop(Item item)
        {
            while (true)
            {
                string input = Utilities.GetUserInput($"\nHow many {item.name}s would you like to sell back to the bazaar? (input x to cancel)");
                if (input == "x")
                {
                    break;
                }

                decimal totalSale;
                int amount = Utilities.StringToInt(input);
                if (StockCheck(item, amount))
                {
                    Item shopItem = bazaarInventory.Find(i => i.name == item.name);
                    totalSale = (item.cost * amount) * sellbackRate;

                    if (item.stock - amount !< 0)
                    {
                        Utilities.StringWithColor("Client does not have enough stock to sell back. Please try again.", ConsoleColor.DarkRed, false);
                        Thread.Sleep(1500);
                        Console.Clear();
                        break;
                    }
                    else
                    {
                        Console.Write($"\nYou're about to sell the above items for: ");
                        Utilities.StringWithColor($"{Utilities.DecimalToString(totalSale)}", ConsoleColor.Cyan, true);
                        Console.Write($"\nYour new balance will be: ");
                        Utilities.StringWithColor($"{Utilities.DecimalToString(client.balance + totalSale)}. ", ConsoleColor.Yellow, true);
                        bool sold = Utilities.InputVerification("Proceed? (y/n)");

                        if (sold)
                        {
                            if (item.stock - amount == 0)
                            {
                                client.inventory.Remove(item);
                                shopItem.stock += amount;
                                client.balance += totalSale;
                            }
                            else if (item.stock - amount > 0)
                            {
                                item.stock -= amount;
                                shopItem.stock += amount;
                                client.balance += totalSale;
                            }
                        }
                    }
                    Utilities.StringWithColor($"\n{amount} {item.name}(s) sold for {Utilities.DecimalToString(totalSale)}.", ConsoleColor.DarkGreen, false);
                    break;
                }
            }
        }

        private Item GetItemFromShop(int inputInt)
        {
            Item selectedItem = null;

            while (selectedItem == null)
            {
                if (inputInt > 0 && inputInt <= bazaarInventory.Count)
                {
                    selectedItem = bazaarInventory[inputInt - 1];
                    if (selectedItem.stock == 0)
                    {
                        Utilities.StringWithColor($"\n{selectedItem.name} out of stock. Please check back later.", ConsoleColor.DarkRed, false);
                        selectedItem = null;
                        break;
                    }
                    Console.WriteLine($"\n{selectedItem.name} selected!");
                }
                else
                {
                    Console.WriteLine("\nPlease try again.\n");
                }
            }
            return selectedItem;
        }

        private void AddItemToBasket(Item item)
        {
            while (true)
            {
                string input = Utilities.GetUserInput($"\nHow many {item.name}s would you like to add to your basket? (input x to cancel)");
                if (input == "x")
                {
                    break;
                }

                if (int.TryParse(input, out int amount))
                {
                    if (StockCheck(item, amount))
                    {
                        Item basketItem = item.Clone();
                        basketItem.stock += amount;
                        item.stock -= amount;

                        client.basket.Add(basketItem);

                        Utilities.StringWithColor($"\n{amount} {basketItem.name}(s) added to basket.", ConsoleColor.DarkGreen, false);

                        break;
                    }
                }
                else
                {
                    Utilities.StringWithColor("\nInvalid input.", ConsoleColor.Red, false);
                }
            }
        }

        private void RemoveItemFromBasket(Item item)
        {
            while (true)
            {
                string input = Utilities.GetUserInput($"\nHow many {item.name}s would you like to remove from basket? (input x to cancel)");
                if (input == "x")
                {
                    break;
                }

                int amount = Utilities.StringToInt(input);
                if (StockCheck(item, amount))
                {
                    Item shopItem = bazaarInventory.Find(i => i.name == item.name);

                    if (item.stock - amount == 0)
                    {
                        client.basket.Remove(item);
                        shopItem.stock += amount;
                    }
                    else if (item.stock - amount > 0)
                    {
                        item.stock -= amount;
                        shopItem.stock += amount;
                    }
                    else if (item.stock - amount < 0)
                    {
                        Utilities.StringWithColor("Basket does not contain specified amount to return. Please try again.", ConsoleColor.DarkRed, false);
                        Thread.Sleep(1500);
                        Console.Clear();
                        break;
                    }
                    Utilities.StringWithColor($"\n{amount} {item.name}(s) removed.", ConsoleColor.Cyan, false);
                    break;
                }
            }
        }

        private Item GetItemFromBasket(int inputInt)
        {
            Item selectedItem = null;

            while (selectedItem == null)
            {
                if (inputInt > 0 && inputInt <= client.basket.Count)
                {
                    selectedItem = client.basket[inputInt - 1];
                    Console.WriteLine($"\n{selectedItem.name} selected!");
                }
                else
                {
                    Console.WriteLine("\nPlease try again.\n");
                }
            }
            return selectedItem;
        }

        private decimal PrintBasket()
        {
            Utilities.StringWithColor($"\n>> Current basket <<", ConsoleColor.Cyan, false);

            if (client.basket.Count == 0)
            {
                Utilities.StringWithColor("\nNo items currently in client.basket.", ConsoleColor.DarkGray, false);
            }

            client.basket.Sort((itemA, itemB) => itemA.itemType.CompareTo(itemB.itemType));

            decimal sumBasket = 0;
            for (int i = 0; i < client.basket.Count; i++)
            {
                Item basketi = client.basket[i];

                Console.WriteLine($"\n{i + 1}. {basketi.name}\nQuantity in basket: {basketi.stock}\nCost (total quantity): {Utilities.DecimalToString(basketi.cost * basketi.stock)}");

                sumBasket += basketi.cost * basketi.stock;
            }

            if (sumBasket > client.balance)
            {
                Utilities.StringWithColor($"\nBasket total: ", ConsoleColor.DarkGray, true);
                Utilities.StringWithColor($"{Utilities.DecimalToString(sumBasket)}", ConsoleColor.DarkRed, true);
                Utilities.StringWithColor($" exceeds client balance: ", ConsoleColor.DarkGray, true);
                Utilities.StringWithColor($"{Utilities.DecimalToString(client.balance)}", ConsoleColor.Yellow, false);
                Utilities.StringWithColor($"Please edit basket until total comes withing range of client balance.", ConsoleColor.DarkGray, false);
            }
            Console.WriteLine($"\nBasket total: {Utilities.DecimalToString(sumBasket)}");
            return sumBasket;
        }

        private bool StockCheck(Item item, int deduction)
        {
            if (item.stock < deduction)
            {
                Utilities.StringWithColor("\nNot enough stock. Please reduce the required quantity.", ConsoleColor.Red, false);
                return false;
            }
            return true;
        }

        private void CheckoutBasket()
        {
            bool checkingOut = true;
            while (checkingOut)
            {
                Console.Clear();
                Utilities.PrintShopBanner();
                decimal totalBasket = PrintBasket();
                client.PrintClientBalance();

                string input = Utilities.GetUserInput("\n1. Purchase items in basket" +
                    "\n2. Input x to return to previous menu");

                
                switch (input)
                {
                    case "1":
                        {
                            if (client.basket.Count > 0)
                            {
                                Console.Write($"\nYou're about to spend ");
                                Utilities.StringWithColor($"{Utilities.DecimalToString(totalBasket)}", ConsoleColor.Cyan, true);
                                Console.Write($" for the above items.");
                                Console.Write($"\nYour new balance will be: ");
                                Utilities.StringWithColor($"{Utilities.DecimalToString(client.balance - totalBasket)}. ", ConsoleColor.Yellow, true);
                                bool purchased = Utilities.InputVerification("Proceed? (y/n)");

                                if (purchased)
                                {
                                    for (int i = 0; i < client.basket.Count; i++)
                                    {
                                        Item basketItem = client.basket[i];
                                        Item inventoryItem = basketItem.Clone();
                                        Item existingItem = client.inventory.Find(n => n.name == inventoryItem.name);

                                        if (client.inventory.Contains(existingItem))
                                        {
                                            existingItem.stock += basketItem.stock;
                                        }
                                        else
                                        {
                                            client.inventory.Add(inventoryItem);
                                            inventoryItem.stock = basketItem.stock;
                                        }
                                    }
                                    client.basket.Clear();
                                    client.balance -= totalBasket;
                                    Utilities.StringWithColor($"\nPurchase completed.", ConsoleColor.DarkGreen, false);
                                    Thread.Sleep(2000);
                                    checkingOut = false;
                                }
                            }
                            else
                            {
                                Utilities.StringWithColor("\nNo items in basket to purchase.", ConsoleColor.Cyan, false);
                                Thread.Sleep(2000);
                            }
                        }
                            
                        break;
                    case "x":
                        {
                            checkingOut = false;
                            Console.Clear();
                        }
                        break;
                    default:
                        {
                            Utilities.StringWithColor("\nInvalid input.", ConsoleColor.Red, false);
                            Thread.Sleep(1500);
                        }
                        break;
                }
            }
        }
    }
}
