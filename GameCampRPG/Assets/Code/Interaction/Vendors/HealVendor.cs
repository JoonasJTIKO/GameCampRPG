using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class HealVendor : BaseVendor, IInteractable
    {
        public bool InRange { get; set; }

        private void Awake()
        {
            SetDialogue();
        }

        protected override void SetDialogue()
        {
            this.SpeakerName = "Hermanni";
            this.DialogueLines = new string[][]
            {
                new string[] {"Terevepp‰ tereve, meitsi on Hermanni ja providaan palveluita",
                "Oisko healin paikka vai aiotko p‰ivitt‰‰ itse‰si?"},
                new string[] {"No eip‰ mulla muuta, terppa!"}
            };

            Debug.Log(DialogueLines[0]);
        }

        private void SetQuestDialogue(string[] text)
        {
            DialogueLines[0] = text;
        }

        public void Interact()
        {
            if (GameInstance.Instance.GetQuestManager().CheckQuestNPC(this))
            {
                SetQuestDialogue(GameInstance.Instance.GetQuestManager().ActiveQuest.QuestDialogue);
            }
            else
            {
                SetDialogue();
            }

            GameInstance.Instance.GetDialogueCanvas().StartDialogue(this, this.DialogueLines[0], true);
        }

        private void OnGUI()
        {
            if (!InRange) return;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            Rect menuRect = new Rect(screenPos.x - xOffset, Screen.height - screenPos.y - yOffset, 200, 100);
            GUI.Label(menuRect, "Press E to interact");
        }
    }
}
