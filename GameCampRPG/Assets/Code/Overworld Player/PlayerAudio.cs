using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class PlayerAudio : MonoBehaviour
    {
        private AudioManager audioManager;

        private void Awake()
        {
            if (GameInstance.Instance == null) return;

            audioManager = GameInstance.Instance.GetAudioManager();
        }

        public void PlayRunSound()
        {
            audioManager.PlayAudioAtLocation(GameSFX.SFX_PLAYER_RUN, transform.position, 0.5f);
        }
    }
}
