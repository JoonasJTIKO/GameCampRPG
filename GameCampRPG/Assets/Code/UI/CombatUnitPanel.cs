using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameCampRPG.UI
{
    public class CombatUnitPanel : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text healthText;

        [SerializeField]
        private TMP_Text cooldownText;

        [SerializeField]
        private Animator animator;

        public void UpdateHealthText(int health)
        {
            healthText.text = "Health: " + health;
        }

        public void UpdateCooldownText(int cooldown)
        {
            cooldownText.text = "Skill: " + cooldown + " turns";
        }

        public void Highlight(bool state)
        {
            if (!transform.parent.gameObject.activeSelf) return;

            animator.SetBool("Selected", state);
        }
    }
}
