using UnityEngine;

namespace GameCampRPG
{
    [System.Serializable]
    public class Item : IItem
    {
        [field: SerializeField]
        public int ID { get; set; }

        [field: SerializeField]
        public string Name { get; set; }

        [field: SerializeField]
        public string Description { get; set; }

        [field: SerializeField]
        public int Amount { get; set; }

        [field: SerializeField]
        public int MaxAmount { get; set; }

        [field: SerializeField]
        public bool IsUnique { get; set; }

        [field: SerializeField]
        public Sprite Icon { get; set; }
    }
}
