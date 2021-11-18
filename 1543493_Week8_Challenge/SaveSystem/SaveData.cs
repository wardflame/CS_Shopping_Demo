using _1543493_Week8_Challenge.ClientContent;
using _1543493_Week8_Challenge.ItemContent;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace _1543493_Week8_Challenge.SaveSystem
{
    public class SaveData
    {
        public Client currentClient;
        public List<Client> clientList;
        public List<Item> bazaarInventory;

        public void SaveShop()
        {
            string saveJson = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(@"Saves\save.json", saveJson);
        }

        public static SaveData LoadShop()
        {
            SaveData saveLoad = JsonConvert.DeserializeObject<SaveData>(File.ReadAllText(@"Saves\save.json"));
            return saveLoad;
        }

        public override string ToString()
        {
            return $"{currentClient}, {clientList}";
        }
    }
}
