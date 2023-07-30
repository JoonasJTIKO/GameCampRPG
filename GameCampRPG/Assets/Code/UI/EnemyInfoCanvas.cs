using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCampRPG.UI
{
    public class EnemyInfoCanvas : MenuCanvas
    {
        [SerializeField]
        private CombatUnitPanel[] evenPanels;

        [SerializeField]
        private CombatUnitPanel[] unevenPanels;

        [SerializeField]
        private Texture snakeSprite;

        [SerializeField]
        private Texture slimeSprite;

        [SerializeField]
        private Texture treeSprite;

        [SerializeField]
        private Texture bossSprite;

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

        public void ActivatePanels(int enemyCount, EnemyType[] types)
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
                    switch (types[index])
                    {
                        case EnemyType.Snake:
                            evenPanels[index + helper].GetComponentInChildren<RawImage>().texture = snakeSprite;
                            break;
                        case EnemyType.Slime:
                            evenPanels[index + helper].GetComponentInChildren<RawImage>().texture = slimeSprite;
                            break;
                        case EnemyType.Tree:
                            evenPanels[index + helper].GetComponentInChildren<RawImage>().texture = treeSprite;
                            break;
                        case EnemyType.Boss:
                            //evenPanels[index + helper].GetComponentInChildren<RawImage>().texture = bossSprite;
                            break;
                    }
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
                    switch (types[index])
                    {
                        case EnemyType.Snake:
                            unevenPanels[index + helper].GetComponentInChildren<RawImage>().texture = snakeSprite;
                            break;
                        case EnemyType.Slime:
                            unevenPanels[index + helper].GetComponentInChildren<RawImage>().texture = slimeSprite;
                            break;
                        case EnemyType.Tree:
                            unevenPanels[index + helper].GetComponentInChildren<RawImage>().texture = treeSprite;
                            break;
                        case EnemyType.Boss:
                            //unevenPanels[index + helper].GetComponentInChildren<RawImage>().texture = bossSprite;
                            break;
                    }
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
