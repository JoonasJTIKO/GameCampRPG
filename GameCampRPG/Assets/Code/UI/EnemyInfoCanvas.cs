using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG.UI
{
    public class EnemyInfoCanvas : MenuCanvas
    {
        [SerializeField]
        private CombatUnitPanel[] evenPanels;

        [SerializeField]
        private CombatUnitPanel[] unevenPanels;

        private bool even = false;

        private int helper = 0;

        public void SetHealth(int unitIndex, int health)
        {
            if (even)
            {
                evenPanels[unitIndex + helper].UpdateHealthText(health);
            }
            else
            {
                unevenPanels[unitIndex + helper].UpdateHealthText(health);
            }
        }

        public void SelectUnit(int unitIndex, bool state)
        {
            if (even)
            {
                evenPanels[unitIndex + helper].Highlight(state);
            }
            else
            {
                unevenPanels[unitIndex + helper].Highlight(state);
            }
        }

        public void ActivatePanels(int enemyCount)
        {
            if (enemyCount == 0) return;

            bool evenAmount = enemyCount % 2 == 0;
            even = evenAmount;
            int index = 0;
            helper = 0;

            if (evenAmount)
            {
                if (enemyCount == 2) helper = 1;
                while (index < enemyCount)
                {
                    evenPanels[index + helper].gameObject.SetActive(true);
                    index++;
                }
            }
            else
            {
                if (enemyCount == 3) helper = 1;
                if (enemyCount == 1) helper = 2;
                while (index < enemyCount)
                {
                    unevenPanels[index + helper].gameObject.SetActive(true);
                    index++;
                }
            }
        }

        public void DeactivatePanels()
        {
            foreach (CombatUnitPanel panel in evenPanels)
            {
                panel.gameObject.SetActive(false);
            }

            foreach (CombatUnitPanel panel in unevenPanels)
            {
                panel.gameObject.SetActive(false);
            }
        }
    }
}
