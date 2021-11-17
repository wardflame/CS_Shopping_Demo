using System;
using System.Collections.Generic;
using System.Text;

namespace _1543493_Week8_Challenge.ItemContent
{
    public class Item
    {
        public string name;
        public string description;
        public ItemType itemType;
        public decimal cost;
        public int stock;

        public enum ItemType
        {
            SmallArms,
            GroundVehicles,
            AerialVehicles
        }

        public Item(string name, string description, ItemType itemType, decimal cost)
        {
            this.name = name;
            this.description = description;
            this.itemType = itemType;
            this.cost = cost;
        }
    }
}
