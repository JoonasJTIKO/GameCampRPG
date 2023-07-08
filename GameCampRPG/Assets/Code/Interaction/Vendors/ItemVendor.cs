using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using GameCampRPG.UI;

namespace GameCampRPG
{
    public class ItemVendor : BaseVendor, IInteractable
    {
        [SerializeField]
        private Item[] items;

        public Inventory Inventory { get; private set; }

        public bool InRange { get; set; }

        private void Awake()
        {
            if (GameInstance.Instance == null) return;

            Inventory = new Inventory(100);

            foreach(Item item in items)
            {
                Inventory.AddItems(item);
            }

            SetDialogue();
        }

        override protected void SetDialogue()
        {
            this.SpeakerName = "Kalle";
            this.DialogueLines = new string[][]
            {
                new string[] {"Tervetulloo miun pieneen kauppaan. Miun nimi on Kalle.",
                "Saisko olla vähän jotain?"},
                new string[] {"Ai haluuks puhuu? No annas kun minä kerron sinulle kaiken loren tästä paikasta",
                "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit..."},
                new string[] {"Kiitos ku ostit kaiken tavaran multa.",
                "Ny mulla on vihdoinkin varaa maksaa mun vuokra"},
                new string[] {"Oolraits, no mukavaa päivän jatkoo siulle"},
            };
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
