using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class CombatEnemyAudio : MonoBehaviour
    {
        private AudioManager audioManager;

        private void Awake()
        {
            if (GameInstance.Instance == null) return;

            audioManager = GameInstance.Instance.GetAudioManager();
        }

        public void PlaySnakeAttack()
        {
            audioManager.PlayAudioAtLocation(GameSFX.SFX_SNAKE_ATTACK, transform.position);
        }

        public void PlaySlimeAttack()
        {
            audioManager.PlayAudioAtLocation(GameSFX.SFX_SLIME_ATTACK, transform.position);
        }

        public void PlayTreeAttack()
        {
            audioManager.PlayAudioAtLocation(GameSFX.SFX_TREE_ATTACK, transform.position);
        }

        public void PlayTakeDamage()
        {
            audioManager.PlayAudioAtLocation(GameSFX.SFX_ENEMY_TAKE_DAMAGE, transform.position);
        }

        public void PlayDie()
        {
            audioManager.PlayAudioAtLocation(GameSFX.SFX_ENEMY_DIE, transform.position);
        }
    }
}
