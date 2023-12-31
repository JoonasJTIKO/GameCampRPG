using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class PlayerInfo : MonoBehaviour
    {
        public Inventory PlayerInventory { get; private set; }

        public PlayerInputs PlayerInputs { get; private set; }

        [SerializeField]
        private List<Item> items;

        public int Money
        {
            get;
            private set;
        }

        private int[] characterHealths = new int[3];
        public int[] CharacterHealths { get { return characterHealths; } }

        private int[] attackStrengths = new int[3];
        public int[] AttackStrengths { get { return attackStrengths; } }

        private int[] skillStrengths = new int[3];
        public int[] SkillStrengths { get { return skillStrengths; } }

        private int[] skillCooldownModifiers = new int[3];
        public int[] SkillCooldownModifiers { get { return skillCooldownModifiers; } }

        private int[] defenses = new int[3];
        public int[] Defenses { get { return defenses; } }

        private void Awake()
        {
            PlayerInputs = new PlayerInputs();
            PlayerInventory = new Inventory(25);

            foreach (Item item in items)
            {
                PlayerInventory.AddItems(item);
            }

            Money = 500;

            for (int i = 0; i < 3; i++)
            {
                SetCharacterHealth(i, 3);
                SetCharacterAttackStrength(i, 1);
                SetCharacterSkillStrength(i, 1);
                SetCharacterDefense(i, 1);
                SetCharacterSkillModifier(i, 1);
            }
        }

        public void SetCharacterHealth(int characterIndex, int value)
        {
            if (value < 1)
            {
                Debug.LogWarning("Health can not be below 1, setting health to 1");
                value = 1;
            }

            characterHealths[characterIndex] = value;
        }

        public void SetCharacterAttackStrength(int characterIndex, int value)
        {
            if (value < 1)
            {
                Debug.LogWarning("Attack strength must be at least 1, setting it to 1");
                value = 1;
            }

            attackStrengths[characterIndex] = value;
        }

        public void SetCharacterSkillStrength(int characterIndex, int value)
        {
            if (value < 1)
            {
                Debug.LogWarning("Skill strength must be at least 1, setting it to 1");
                value = 1;
            }

            skillStrengths[characterIndex] = value;
        }

        public void SetCharacterSkillModifier(int characterIndex, int value)
        {
            if (value < 1)
            {
                Debug.LogWarning("Skill modifier must be at least 1, setting it to 1");
                value = 1;
            }

            skillCooldownModifiers[characterIndex] = value;
        }

        public void SetCharacterDefense(int characterIndex, int value)
        {
            if (value < 1)
            {
                Debug.LogWarning("Defense must be at least 1, setting it to 1");
                value = 1;
            }

            defenses[characterIndex] = value;
        }

        public void AddItemToInventory(Item item)
        {
            int amount = PlayerInventory.AddItems(item);
            items = PlayerInventory.ShowAllItems();
            if (amount != 0)
            {
                Item copy = new Item();
                copy.ID = item.ID;
                copy.Amount = amount;
                GameInstance.Instance.GetQuestManager().CheckForQuestProgress(copy);
            }
;
        }

        public void RemoveItem(Item item)
        {
            PlayerInventory.GetItem(item);
        }

        public void AddMoney(int amount)
        {
            Money += amount;
        }

        public void RemoveMoney(int amount)
        {
            Money -= amount;
            if (Money < 0) Money = 0;
        }

        public void IncreaseStat(Item.ItemStat stat, int characterIndex)
        {
            if (stat == Item.ItemStat.None) return;

            switch (stat)
            {
                case Item.ItemStat.IncreaseDamage:
                    SetCharacterAttackStrength(characterIndex, attackStrengths[characterIndex] + 1);
                    break;
                case Item.ItemStat.IncreaseDefense:
                    SetCharacterDefense(characterIndex, defenses[characterIndex] + 1);
                    break;
                case Item.ItemStat.IncreaseSkill:
                    SetCharacterSkillStrength(characterIndex, skillStrengths[characterIndex] + 1);
                    break;
                case Item.ItemStat.DecreaseCooldown:
                    SetCharacterSkillModifier(characterIndex, skillCooldownModifiers[characterIndex] + 1);
                    break;
            }
        }

        public void DecreaseStat(Item.ItemStat stat, int characterIndex)
        {
            if (stat == Item.ItemStat.None) return;

            switch (stat)
            {
                case Item.ItemStat.IncreaseDamage:
                    SetCharacterAttackStrength(characterIndex, attackStrengths[characterIndex] - 1);
                    break;
                case Item.ItemStat.IncreaseDefense:
                    SetCharacterDefense(characterIndex, defenses[characterIndex] - 1);
                    break;
                case Item.ItemStat.IncreaseSkill:
                    SetCharacterSkillStrength(characterIndex, skillStrengths[characterIndex] - 1);
                    break;
                case Item.ItemStat.DecreaseCooldown:
                    SetCharacterSkillModifier(characterIndex, skillCooldownModifiers[characterIndex] - 1);
                    break;
            }
        }

        public void UseItem(Item.ConsumableType usableType, int characterIndex)
        {
            if (usableType == Item.ConsumableType.None) return;

            switch (usableType)
            {
                case Item.ConsumableType.Heal:
                    SetCharacterHealth(characterIndex, characterHealths[characterIndex] + 1);
                    break;
                case Item.ConsumableType.HealAll:
                    for (int i = 0; i < 3; i++)
                    {
                        SetCharacterHealth(i, characterHealths[i] + 1);
                    }
                    break;
                case Item.ConsumableType.LowerCooldown:
                    SetCharacterSkillModifier(characterIndex, skillCooldownModifiers[characterIndex] + 1);
                    break;
            }
        }
    }
}
