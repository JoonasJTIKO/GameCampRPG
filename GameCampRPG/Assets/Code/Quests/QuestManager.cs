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

        private QuestObjective activeQuest = null;

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
            activeQuest = questObjectives[ID];

            if (activeQuest.Type == QuestType.SpeakWithNPC)
            {
                questCanvas.UpdateText(activeQuest.Text, 0, 1);
            }
            else
            {
                questCanvas.UpdateText(activeQuest.Text, activeQuest.RequiredItems[0].Amount, activeQuest.RequiredItems[0].MaxAmount);
            }

            ID++;
        }

        public void CheckForQuestProgress(IItem obtainedItem = null, BaseVendor vendor = null)
        {
            if (obtainedItem != null && activeQuest.Type == QuestType.CollectItems)
            {
                foreach (IItem item in activeQuest.RequiredItems)
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

                questCanvas.UpdateText(activeQuest.Text, activeQuest.RequiredItems[0].Amount, activeQuest.RequiredItems[0].MaxAmount);

                int counter = 0;
                foreach (IItem item in activeQuest.RequiredItems)
                {
                    if (item.Amount != item.MaxAmount) break;

                    counter++;
                }

                if (counter == activeQuest.RequiredItems.Length)
                {
                    StartNextQuest();
                }
            }
            else if (vendor != null && activeQuest.Type == QuestType.SpeakWithNPC)
            {
                if (vendor.SpeakerName.ToLower() == activeQuest.NPCName.ToLower())
                {
                    StartNextQuest();
                }
            }
        }
    }
}
