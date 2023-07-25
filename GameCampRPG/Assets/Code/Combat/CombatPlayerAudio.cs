using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class CombatPlayerAudio : MonoBehaviour
    {
        private AudioManager audioManager;

        private void Awake()
        {
            if (GameInstance.Instance == null) return;

            audioManager = GameInstance.Instance.GetAudioManager();
        }

        public void PlayRogueAttack()
        {
            audioManager.PlayAudioAtLocation(GameSFX.SFX_ROGUE_ATTACK, transform.position);
        }
        
        public void PlayRogueSkill()
        {
            audioManager.PlayAudioAtLocation(GameSFX.SFX_ROGUE_SKILL, transform.position);
        }

        public void PlayKnightAttack()
        {
            audioManager.PlayAudioAtLocation(GameSFX.SFX_KNIGHT_ATTACK, transform.position);
        }

        public void PlayKnightSkill()
        {
            audioManager.PlayAudioAtLocation(GameSFX.SFX_KNIGHT_SKILL, transform.position);
        }

        public void PlayMageAttack()
        {
            audioManager.PlayAudioAtLocation(GameSFX.SFX_MAGE_ATTACK, transform.position);
        }

        public void PlayMageSkill()
        {
            audioManager.PlayAudioAtLocation(GameSFX.SFX_MAGE_SKILL, transform.position);
        }

        public void PlayJump()
        {
            audioManager.PlayAudioAtLocation(GameSFX.SFX_COMBAT_PLAYER_JUMP, transform.position);
        }

        public void PlayTakeDamage()
        {
            audioManager.PlayAudioAtLocation(GameSFX.SFX_PLAYER_TAKE_DAMAGE, transform.position);
        }

        public void PlayDie()
        {
            audioManager.PlayAudioAtLocation(GameSFX.SFX_PLAYER_DIE, transform.position);
        }
    }
}
