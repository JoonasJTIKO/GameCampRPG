using UnityEngine;

namespace GameCampRPG.Quests
{
    public enum QuestType
    {
        SpeakWithNPC,
        CollectItems
    }

    [System.Serializable]
    public class QuestObjective
    {
        [SerializeField]
        public int ID;

        [SerializeField]
        public string Text;

        [SerializeField]
        public QuestType Type;

        [SerializeField]
        public string NPCName;

        [SerializeField]
        public DialogueLine[] QuestDialogue;

        [SerializeField]
        public Item[] RequiredItems;
    }
}
