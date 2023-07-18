using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCampRPG.UI
{
    public class PlayerCombatCanvas : MenuCanvas
    {
        [SerializeField]
        private TMP_Text currentUnitText;

        [SerializeField]
        private TMP_Text currentCooldownText;

        [SerializeField]
        private CombatUnitPanel roguePanel;

        [SerializeField]
        private CombatUnitPanel knightPanel;

        [SerializeField]
        private CombatUnitPanel magePanel;

        public static event Action OnAttackPressed, OnSkillPressed, OnBlockPressed, OnMovePressed;


        public void SetUnitText(string text)
        {
            currentUnitText.text = text;
        }

        public void SelectUnit(int unitIndex, bool state)
        {
            eventSystem.SetSelectedGameObject(initialSelectedObject);

            switch (unitIndex)
            {
                case 0:
                    roguePanel.Highlight(state);
                    break;
                case 1:
                    knightPanel.Highlight(state);
                    break;
                case 2:
                    magePanel.Highlight(state);
                    break;
            }
        }

        public void SetHealthText(int unitIndex, int health)
        {
            switch (unitIndex)
            {
                case 0:
                    roguePanel.UpdateHealthText(health);
                    break;
                case 1:
                    knightPanel.UpdateHealthText(health);
                    break;
                case 2:
                    magePanel.UpdateHealthText(health);
                    break;
            }
        }

        public void SetCooldownText(int unitIndex, int cooldown)
        {
            switch (unitIndex)
            {
                case 0:
                    roguePanel.UpdateCooldownText(cooldown);
                    break;
                case 1:
                    knightPanel.UpdateCooldownText(cooldown);
                    break;
                case 2:
                    magePanel.UpdateCooldownText(cooldown);
                    break;
            }
        }

        public void AttackPressed()
        {
            OnAttackPressed?.Invoke();
        }

        public void SkillPressed()
        {
            OnSkillPressed?.Invoke();
        }

        public void BlockPressed()
        {
            OnBlockPressed?.Invoke();
        }

        public void MovePressed()
        {
            OnMovePressed?.Invoke();
        }
    }
}
