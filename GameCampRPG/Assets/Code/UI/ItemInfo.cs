using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GameCampRPG.UI
{
    public class ItemInfo : MenuCanvas
    {
        private TextMeshProUGUI[] texts;

        private void Awake()
        {
            texts = GetComponentsInChildren<TextMeshProUGUI>();
        }
        
        public void SetInfo(IItem item)
        {
            texts[0].text = item.Name;
            texts[1].text = item.Description;
        }
    }
}
