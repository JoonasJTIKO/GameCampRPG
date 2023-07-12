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
            PlayerInventory = new Inventory(50);

            foreach (Item item in items)
            {
                PlayerInventory.AddItems(item);
            }
        }

        public void SetCharacterHealth(int characterIndex, int value)
        {
            if (value < 0)
            {
                Debug.LogWarning("Health can not be below 0, setting health to 0");
                value = 0;
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

        public void AddItemToInventory(IItem item)
        {
            int amount = PlayerInventory.AddItems(item);
            if (amount != 0)
            {
                Item copy = new Item();
                copy.ID = item.ID;
                copy.Amount = amount;
                GameInstance.Instance.GetQuestManager().CheckForQuestProgress(copy);
            }
;       }

        public void IncreaseStat(Item.ItemStat stat, int characterIndex)
        {
            if (stat == Item.ItemStat.None) return;

            switch (stat)
            {
                case Item.ItemStat.IncreaseDamage:
                    SetCharacterAttackStrength(attackStrengths[characterIndex] + 1, characterIndex);
                    break;
                case Item.ItemStat.IncreaseDefense:
                    SetCharacterDefense(defenses[characterIndex] + 1, characterIndex);
                    break;
                case Item.ItemStat.IncreaseSkill:
                    SetCharacterSkillStrength(skillStrengths[characterIndex] + 1, characterIndex);
                    break;
                case Item.ItemStat.DecreaseCooldown:
                    SetCharacterSkillModifier(skillCooldownModifiers[characterIndex] + 1, characterIndex);
                    break;
            }
        }

        public void DecreaseStat(Item.ItemStat stat, int characterIndex)
        {
            if (stat == Item.ItemStat.None) return;

            switch (stat)
            {
                case Item.ItemStat.IncreaseDamage:
                    SetCharacterAttackStrength(attackStrengths[characterIndex] - 1, characterIndex);
                    break;
                case Item.ItemStat.IncreaseDefense:
                    SetCharacterDefense(defenses[characterIndex] - 1, characterIndex);
                    break;
                case Item.ItemStat.IncreaseSkill:
                    SetCharacterSkillStrength(skillStrengths[characterIndex] - 1, characterIndex);
                    break;
                case Item.ItemStat.DecreaseCooldown:
                    SetCharacterSkillModifier(skillCooldownModifiers[characterIndex] - 1, characterIndex);
                    break;
            }
        }
    }
}
