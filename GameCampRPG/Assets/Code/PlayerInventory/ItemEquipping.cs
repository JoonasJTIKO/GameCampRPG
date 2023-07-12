using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class ItemEquipping : MonoBehaviour
    {
        private PlayerInfo playerInfo;

        public Item EquippedKnightWeapon { get; private set; }
        public Item EquippedKnightArmor { get; private set; }
        public Item EquippedRogueWeapon { get; private set; }
        public Item EquippedRogueArmor { get; private set; }
        public Item EquippedMageWeapon { get; private set; }
        public Item EquippedMageArmor { get; private set; }

        private void Awake()
        {
            if (GameInstance.Instance == null) return;

            playerInfo = GameInstance.Instance.GetPlayerInfo();
        }

        public void EquipItem(Item item, int characterIndex)
        {
            Item.EquippableType newItemType = item.equippableType;

            playerInfo.IncreaseStat(item.Stat1, characterIndex);
            playerInfo.IncreaseStat(item.Stat2, characterIndex);

            switch (newItemType)
            {
                case Item.EquippableType.Weapon:
                    if (characterIndex == 0)
                    {
                        if (EquippedRogueWeapon != null) UnequipItem(EquippedRogueWeapon, 1);

                        EquippedRogueWeapon = item;
                    }
                    else if (characterIndex == 1)
                    {
                        if (EquippedKnightWeapon != null) UnequipItem(EquippedKnightWeapon, 0);

                        EquippedKnightWeapon = item;
                    }
                    else if (characterIndex == 2)
                    {
                        if (EquippedMageWeapon != null) UnequipItem(EquippedMageWeapon, 2);

                        EquippedMageWeapon = item;
                    }

                    break;

                case Item.EquippableType.Armor:
                    if (characterIndex == 0)
                    {
                        if (EquippedRogueArmor != null) UnequipItem(EquippedRogueArmor, 1);

                        EquippedRogueArmor = item;
                    }
                    else if (characterIndex == 1)
                    {
                        if (EquippedKnightArmor != null) UnequipItem(EquippedKnightArmor, 0);

                        EquippedKnightArmor = item;
                    }
                    else if (characterIndex == 2)
                    {
                        if (EquippedMageArmor != null) UnequipItem(EquippedMageArmor, 2);

                        EquippedMageArmor = item;
                    }

                    break;
            }
        }

        public void UnequipItem(Item item, int characterIndex)
        {
            Item.EquippableType itemType = item.equippableType;

            switch (itemType)
            {
                case Item.EquippableType.Weapon:
                    if (characterIndex == 0)
                    {
                        EquippedRogueWeapon = null;
                    }
                    else if (characterIndex == 1)
                    {
                        EquippedKnightWeapon = null;
                    }
                    else if (characterIndex == 2)
                    {
                        EquippedMageWeapon = null;
                    }

                    break;

                case Item.EquippableType.Armor:
                    if (characterIndex == 0)
                    {
                        EquippedRogueArmor = null;
                    }
                    else if (characterIndex == 1)
                    {
                        EquippedKnightArmor = null;
                    }
                    else if (characterIndex == 2)
                    {
                        EquippedMageArmor = null;
                    }

                    break;
            }
        }
    }
}
