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

        /// <summary>
        /// Take data from object, serialise and save it to Saves\save.json.
        /// </summary>
        public void SaveShop()
        {
            string saveJson = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(@"Saves\save.json", saveJson);
        }

        /// <summary>
        /// Deserialise data from Saves\save.json and return it as an object.
        /// </summary>
        /// <returns></returns>
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
