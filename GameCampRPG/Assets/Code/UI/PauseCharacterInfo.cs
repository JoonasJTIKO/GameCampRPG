using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameCampRPG
{
    public class PauseCharacterInfo : MonoBehaviour
    {
        [SerializeField]
        private int characterIndex;

        [SerializeField]
        private TMP_Text healthText;
        [SerializeField]
        private TMP_Text attackText;
        [SerializeField]
        private TMP_Text skillText;
        [SerializeField]
        private TMP_Text defenseText;

        private PlayerInfo playerInfo;

        private void Start()
        {
            playerInfo = GameInstance.Instance.GetPlayerInfo();
        }

        private void UpdateTexts()
        {
            healthText.text = "Health: " + playerInfo.CharacterHealths[characterIndex];
            attackText.text = "Attack: " + playerInfo.AttackStrengths[characterIndex];
            skillText.text = "Skill: " + playerInfo.SkillStrengths[characterIndex];
            defenseText.text = "Defense: " + playerInfo.Defenses[characterIndex];
        }
    }
}
