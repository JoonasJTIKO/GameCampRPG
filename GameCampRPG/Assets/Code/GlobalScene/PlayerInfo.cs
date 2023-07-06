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

        private int[] charaterHealths = new int[3];
        public int[] CharacterHealths { get { return charaterHealths; } }

        private int[] attackStrengths = new int[3];
        public int[] AttackStrengths { get { return attackStrengths; } }

        private int[] skillStrengths = new int[3];
        public int[] SkillStrengths { get { return skillStrengths; } }

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

            charaterHealths[characterIndex] = value;
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
    }
}
