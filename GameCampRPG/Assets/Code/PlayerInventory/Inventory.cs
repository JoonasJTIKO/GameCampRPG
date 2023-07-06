using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class Inventory
    {
        public List<IItem> Items
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

            Items = new List<IItem>();
        }

        /// <summary>
        /// Adds item(s) to this inventory, the item stores it's amount
        /// </summary>
        /// <param name="item">Item(s) to be added</param>
        /// <returns>The amount of the given item that were added. Therefore 0 means adding failed</returns>
        public int AddItems(IItem item)
        {
            IItem existing = null;
            foreach (IItem inspectedItem in Items)
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

        public IItem GetItem(IItem item, int amount = 1)
        {
            return GetItem(item.ID, amount);
        }

        public IItem GetItem(int ID, int amount = 1)
        {
            IItem returnItem = null;
            IItem removeItem = null;

            foreach (IItem inspectedItem in Items)
            {
                if (inspectedItem.ID == ID)
                {
                    returnItem = new Item()
                    {
                        ID = inspectedItem.ID,
                        Name = inspectedItem.Name,
                        Description = inspectedItem.Description,
                        MaxAmount = inspectedItem.MaxAmount,
                        IsUnique = inspectedItem.IsUnique,
                        Icon = inspectedItem.Icon
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

        public List<IItem> GetAllItems()
        {
            List<IItem> items = new List<IItem>(Items);
            Items.Clear();
            return items;
        }

        public List<IItem> ShowAllItems()
        {
            List<IItem> items = new List<IItem>(Items);
            return items;
        }
    }
}
