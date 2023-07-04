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
            this.DialogueLines = new string[][]
            {
                new string[] {"Jaahas, mitäs sitä täältä ollaan oikein hakemassa?",
                "Mun nimi on Hirvi ja kaikki mitä multa löytyy on laillisesti hankittu",
                "Saisko sulle olla jotain tiskin alta?"},
                new string[] {"Kiitza sulle rahoista.", "Et tuu ikinä saamaan niitä enää takasin XD"},
                new string[] {"Hyvästi, ja jos kerrot poliisille nii mä tiiän mis asut."}
            };
        }

        public void Interact()
        {
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
