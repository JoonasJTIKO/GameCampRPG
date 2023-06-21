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

        public string SpeakerName { get; protected set; }

        public string[][] DialogueLines { get; protected set; }

        abstract protected void SetDialogue();
    }
}
