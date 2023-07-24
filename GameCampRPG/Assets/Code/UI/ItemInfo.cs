using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GameCampRPG.UI
{
    public class ItemInfo : MenuCanvas
    {
        public void SetInfo(Item item)
        {
            TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = item.Name;
            texts[1].text = item.Description;
        }

        public void ClearInfo()
        {
            TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = "";
            texts[1].text = "";
        }
    }
}
