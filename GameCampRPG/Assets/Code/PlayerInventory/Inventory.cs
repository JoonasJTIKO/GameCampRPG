using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class Inventory
    {
        public List<Item> Items
        {
            get;
        }

        public int ItemAmount
        {
            get { return Items.Count; }
        }

        public int MaxAmount
        {
            get;
        }

        public Inventory(int maxAmount)
        {
            MaxAmount = maxAmount;

            Items = new List<Item>();
        }

        /// <summary>
        /// Adds item(s) to this inventory, the item stores it's amount
        /// </summary>
        /// <param name="item">Item(s) to be added</param>
        /// <returns>The amount of the given item that were added. Therefore 0 means adding failed</returns>
        public int AddItems(Item item)
        {
            Item existing = null;
            foreach (Item inspectedItem in Items)
            {
                if (inspectedItem.ID == item.ID)
                {
                    existing = inspectedItem;
                    break;
                }
            }

            if (existing != null && !existing.IsUnique)
            {
                int amount = item.Amount;
                while (amount + existing.Amount > existing.MaxAmount)
                {
                    amount--;
                }
                existing.Amount += amount;
                return amount;
            }
            else if (ItemAmount < MaxAmount)
            {
                Items.Add(item);
                return item.Amount;
            }

            return 0;
        }

        public Item GetItem(Item item, int amount = 1)
        {
            return GetItem(item.ID, amount);
        }

        public Item GetItem(int ID, int amount = 1)
        {
            Item returnItem = null;
            Item removeItem = null;

            foreach (Item inspectedItem in Items)
            {
                if (inspectedItem.ID == ID)
                {
                    returnItem = new Item()
                    {
                        ID = inspectedItem.ID,
                        Name = inspectedItem.Name,
                        Description = inspectedItem.Description,
                        Price = inspectedItem.Price,
                        MaxAmount = inspectedItem.MaxAmount,
                        IsUnique = inspectedItem.IsUnique,
                        Icon = inspectedItem.Icon,
                        Equippable = inspectedItem.Equippable,
                        EquipCharacter = inspectedItem.EquipCharacter,
                        Stat1 = inspectedItem.Stat1,
                        Stat2 = inspectedItem.Stat2,
                        EquipType = inspectedItem.EquipType
                    };

                    if (inspectedItem.Amount > amount)
                    {
                        inspectedItem.Amount -= amount;
                        returnItem.Amount = amount;
                    }
                    else if (inspectedItem.Amount == amount)
                    {
                        returnItem.Amount = amount;
                        removeItem = inspectedItem;
                    }
                    else
                    {
                        returnItem.Amount = inspectedItem.Amount;
                        removeItem = inspectedItem;
                    }

                    break;
                }
            }

            if (removeItem != null)
            {
                Items.Remove(removeItem);
            }

            return returnItem;
        }

        public List<Item> GetAllItems()
        {
            List<Item> items = new List<Item>(Items);
            Items.Clear();
            return items;
        }

        public List<Item> ShowAllItems()
        {
            List<Item> items = new List<Item>(Items);
            return items;
        }
    }
}
