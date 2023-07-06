using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameCampRPG.UI
{
    public class QuestCanvas : MenuCanvas
    {
        [SerializeField]
        private TMP_Text questText;

        [SerializeField]
        private TMP_Text questProgress;

        public void UpdateText(string text = null, int progress = -1, int maxProgress = 1)
        {
            if (text != null) questText.text = text;

            if (progress > -1)
            {
                string progressText = progress + " / " + maxProgress;
                questProgress.text = progressText;
            }
        }
    }
}
