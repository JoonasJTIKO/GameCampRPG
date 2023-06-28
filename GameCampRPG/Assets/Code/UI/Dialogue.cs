using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

namespace GameCampRPG.UI
{
    public class Dialogue : MenuCanvas
    {
        [SerializeField]
        private TextMeshProUGUI dialogueText;

        [SerializeField]
        private TextMeshProUGUI speaker;

        [SerializeField]
        private float textSpeed;

        [SerializeField]
        private InteractionMenu interactionMenu;

        private PlayerInputs inputs;

        private InputAction select;

        private string[] lines;

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
        }

        private void OnDisable()
        {
            if (inputs == null) return;

            inputs.UI.Disable();
            select.performed -= PerformSkip;
        }

        private void PerformSkip(InputAction.CallbackContext callback)
        {
            if (dialogueText.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopCoroutine(typeLine);
                dialogueText.text = lines[index];
            }
        }

        //TODO: passing the sprite of the gameobject to the dialogue
        public void StartDialogue(BaseVendor caller, string[] lines, bool openMenu = false)
        {
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

        public void SetText(string text)
        {
            if (typeLine != null)
            {
                StopCoroutine(typeLine);
            }
            dialogueText.text = text;
        }

        IEnumerator TypeLine()
        {
            foreach (char c in lines[index].ToCharArray())
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
