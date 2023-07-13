using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class BlackMarketVendor : BaseVendor, IInteractable
    {
        [SerializeField]
        private Item[] items;

        public Inventory Inventory { get; private set; }

        public bool InRange { get; set; }

        private void Awake()
        {
            if (GameInstance.Instance == null) return;

            Inventory = new Inventory(100);

            foreach (Item item in items)
            {
                Inventory.AddItems(item);
            }

            SetDialogue();
        }

        override protected void SetDialogue()
        {
            this.SpeakerName = "Hirvi";
            this.DialogueLines = new DialogueLine[4][];


            DialogueLines[0] = introDialogue;
            DialogueLines[1] = talkDialogue;
            DialogueLines[2] = specialDialogue;
            DialogueLines[3] = leaveDialogue;
        }

        private void SetQuestDialogue(DialogueLine[] text)
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
