using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public abstract class BaseVendor : MonoBehaviour
    {
        [SerializeField]
        protected float xOffset = 55f;

        [SerializeField]
        protected float yOffset = 100f;

        [SerializeField]
        protected DialogueLine[] introDialogue;

        [SerializeField]
        protected DialogueLine[] talkDialogue;

        [SerializeField]
        protected DialogueLine[] specialDialogue;

        [SerializeField]
        protected DialogueLine[] leaveDialogue;

        public string SpeakerName { get; protected set; }

        public DialogueLine[][] DialogueLines { get; protected set; }

        public Sprite Icon { get; protected set; }

        abstract protected void SetDialogue();
    }
}
