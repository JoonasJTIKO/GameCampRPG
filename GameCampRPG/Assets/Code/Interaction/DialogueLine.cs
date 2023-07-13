using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    [System.Serializable]
    public class DialogueLine
    {
        [SerializeField]
        private Sprite speakerIcon;

        [SerializeField]
        private string speakerName;

        [SerializeField]
        private string dialogueLine;

        public Sprite SpeakerIcon { get { return speakerIcon; } }
        public string SpeakerName { get { return speakerName; } }
        public string Dialogue { get { return dialogueLine; } }
    }
}
