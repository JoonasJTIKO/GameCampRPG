using GameCampRPG.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG.Quests
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField]
        private QuestObjective[] questObjectives;

        public QuestObjective ActiveQuest { get; private set; }

        private QuestCanvas questCanvas = null;

        private int ID = 0;

        private void Awake()
        {
            questCanvas = GameInstance.Instance.GetQuestCanvas();
        }

        private void Start()
        {
            StartNextQuest();
        }

        private void StartNextQuest()
        {
            ActiveQuest = questObjectives[ID];

            if (ActiveQuest.Type == QuestType.SpeakWithNPC)
            {
                questCanvas.UpdateText(ActiveQuest.Text, 0, 1);
            }
            else
            {
                questCanvas.UpdateText(ActiveQuest.Text, ActiveQuest.RequiredItems[0].Amount, ActiveQuest.RequiredItems[0].MaxAmount);
            }

            ID++;
        }

        public bool CheckQuestNPC(BaseVendor vendor)
        {
            return vendor.SpeakerName.ToLower() == ActiveQuest.NPCName.ToLower();
        }

        public void CheckForQuestProgress(IItem obtainedItem = null, BaseVendor vendor = null)
        {
            if (obtainedItem != null && ActiveQuest.Type == QuestType.CollectItems)
            {
                foreach (IItem item in ActiveQuest.RequiredItems)
                {
                    if (item.Amount == item.MaxAmount) continue;

                    if (item.ID == obtainedItem.ID)
                    {
                        item.Amount += obtainedItem.Amount;

                        if (item.Amount >= item.MaxAmount)
                        {
                            item.Amount = item.MaxAmount;
                        }

                        break;
                    }
                }

                questCanvas.UpdateText(ActiveQuest.Text, ActiveQuest.RequiredItems[0].Amount, ActiveQuest.RequiredItems[0].MaxAmount);

                int counter = 0;
                foreach (IItem item in ActiveQuest.RequiredItems)
                {
                    if (item.Amount != item.MaxAmount) break;

                    counter++;
                }

                if (counter == ActiveQuest.RequiredItems.Length)
                {
                    StartNextQuest();
                }
            }
            else if (vendor != null && ActiveQuest.Type == QuestType.SpeakWithNPC)
            {
                if (vendor.SpeakerName.ToLower() == ActiveQuest.NPCName.ToLower())
                {
                    StartNextQuest();
                }
            }
        }
    }
}
