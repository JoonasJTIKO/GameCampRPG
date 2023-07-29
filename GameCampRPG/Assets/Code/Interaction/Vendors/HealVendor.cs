using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class HealVendor : BaseVendor, IInteractable
    {
        private void Awake()
        {
            SetDialogue();
        }

        protected override void SetDialogue()
        {
            this.SpeakerName = "Noah";
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

        public void PlayerEnterRange(bool entered)
        {
            interactPrompt.enabled = entered;
        }
    }
}
