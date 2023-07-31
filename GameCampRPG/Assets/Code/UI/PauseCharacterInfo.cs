using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameCampRPG
{
    public class PauseCharacterInfo : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text[] healthTexts;
        [SerializeField]
        private TMP_Text[] attackTexts;
        [SerializeField]
        private TMP_Text[] skillTexts;
        [SerializeField]
        private TMP_Text[] defenseTexts;

        private PlayerInfo playerInfo;

        private void Start()
        {
            playerInfo = GameInstance.Instance.GetPlayerInfo();
            UpdateTexts();
        }

        private void OnEnable()
        {
            if (playerInfo == null) return;
            UpdateTexts();
        }

        private void UpdateTexts()
        {
            for (int i = 0; i < 3; i++)
            {
                healthTexts[i].text = "Health: " + playerInfo.CharacterHealths[i];
                attackTexts[i].text = "Attack: " + playerInfo.AttackStrengths[i];
                skillTexts[i].text = "Skill: " + playerInfo.SkillStrengths[i];
                defenseTexts[i].text = "Defense: " + playerInfo.Defenses[i];
            }
        }
    }
}
