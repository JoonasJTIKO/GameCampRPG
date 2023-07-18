using UnityEngine;

namespace GameCampRPG
{
    [System.Serializable]
    public class Item : IItem
    {
        public enum ItemStat
        {
            None = 0,
            IncreaseDamage = 1,
            IncreaseDefense = 2,
            IncreaseSkill = 3,
            DecreaseCooldown = 4
        }

        public enum EquippableType
        {
            Weapon = 0,
            Armor = 1
        }

        [field: SerializeField]
        public int ID { get; set; }

        [field: SerializeField]
        public string Name { get; set; }

        [field: SerializeField]
        public string Description { get; set; }

        [field: SerializeField]
        public int Price { get; set; }

        [field: SerializeField]
        public int Amount { get; set; }

        [field: SerializeField]
        public int MaxAmount { get; set; }

        [field: SerializeField]
        public bool IsUnique { get; set; }

        [field: SerializeField]
        public Sprite Icon { get; set; }

        [field: SerializeField]
        public bool Equippable { get; set; }

        [field: SerializeField]
        public ItemStat Stat1 { get; set; }

        [field: SerializeField]
        public ItemStat Stat2 { get; set; }

        [field: SerializeField]
        public EquippableType equippableType { get; set; }
    }
}
