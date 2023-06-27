using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameCampRPG.UI
{
    public class PlayerCombatCanvas : MenuCanvas
    {
        [SerializeField]
        private TMP_Text currentUnitText;

        [SerializeField]
        private TMP_Text currentCooldownText;

        public static event Action OnAttackPressed, OnSkillPressed, OnBlockPressed, OnMovePressed;

        public void SetUnitText(string text)
        {
            currentUnitText.text = text;
        }

        public void SetCooldownText(int number)
        {
            currentCooldownText.text = number.ToString();
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
