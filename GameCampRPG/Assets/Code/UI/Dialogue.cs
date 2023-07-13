using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace GameCampRPG.UI
{
    public class Dialogue : MenuCanvas
    {
        [SerializeField]
        private TextMeshProUGUI dialogueText;

        [SerializeField]
        private TextMeshProUGUI speaker;

        [SerializeField]
        private Image speakerIcon;

        [SerializeField]
        private float textSpeed;

        [SerializeField]
        private InteractionMenu interactionMenu;

        private PlayerInputs inputs;

        private InputAction select;

        private DialogueLine[] lines;

        private int index;

        private Coroutine typeLine;

        private BaseVendor caller;

        private bool openMenu;

        private void Start()
        {
            Hide();

            if (GameInstance.Instance == null) return;
            inputs = GameInstance.Instance.GetPlayerInfo().PlayerInputs;
            select = inputs.UI.Select;
        }

        private void OnEnable()
        {
            if (inputs == null) return;

            inputs.UI.Enable();
            //gameObject.GetComponent<SpriteRenderer>().sprite = speakerIcon;
        }

        private void OnDisable()
        {
            if (inputs == null) return;

            inputs.UI.Disable();
            select.performed -= PerformSkip;
        }

        private void PerformSkip(InputAction.CallbackContext callback)
        {
            if (dialogueText.text == lines[index].Dialogue)
            {
                NextLine();
            }
            else
            {
                StopCoroutine(typeLine);
                dialogueText.text = lines[index].Dialogue;
            }
        }

        //TODO: passing the sprite of the gameobject to the dialogue
        public void StartDialogue(BaseVendor caller, DialogueLine[] lines, bool openMenu = false)
        {
            //this.speakerIcon = caller.Icon;
            Show();
            select.performed += PerformSkip;

            if (typeLine != null)
            {
                StopCoroutine(typeLine);
            }

            inputs.Overworld.Disable();
            dialogueText.text = string.Empty;
            this.openMenu = openMenu;
            this.caller = caller;
            this.speaker.text = caller.SpeakerName;
            this.lines = lines;
            this.index = 0;
            typeLine = StartCoroutine(TypeLine());
        }

        public void SetText(string text/*, Sprite itemIcon*/)
        {
            if (typeLine != null)
            {
                StopCoroutine(typeLine);
            }
            //this.speakerIcon = itemIcon;
            dialogueText.text = text;
        }

        IEnumerator TypeLine()
        {
            this.speakerIcon.sprite = lines[index].SpeakerIcon;
            this.speaker.text = lines[index].SpeakerName;

            foreach (char c in lines[index].Dialogue.ToCharArray())
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(textSpeed / 60);
            }
        }

        private void NextLine()
        {
            StopCoroutine(typeLine);

            if (index < lines.Length - 1)
            {
                index++;
                if (index == lines.Length - 1 && openMenu)
                {
                    interactionMenu.OpenMenu(caller);
                    select.performed -= PerformSkip;
                }
                dialogueText.text = string.Empty;
                typeLine = StartCoroutine(TypeLine());
            }
            else
            {
                inputs.Overworld.Enable();
                Hide();
            }
        }
    }
}
