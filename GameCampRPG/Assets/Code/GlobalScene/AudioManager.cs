using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace GameCampRPG
{
    #region Audio ENUM
    public enum GameSFX
    {
        // PLAYER SFX
        SFX_PLAYER_RUN,
        SFX_COMBAT_PLAYER_JUMP,
        SFX_ROGUE_ATTACK,
        SFX_ROGUE_SKILL,
        SFX_KNIGHT_ATTACK,
        SFX_KNIGHT_SKILL,
        SFX_MAGE_ATTACK,
        SFX_MAGE_SKILL,
        SFX_PLAYER_TAKE_DAMAGE,
        SFX_PLAYER_DIE,

        // ENEMY SFX
        SFX_SNAKE_ATTACK,
        SFX_SLIME_ATTACK,
        SFX_TREE_ATTACK,
        SFX_ENEMY_TAKE_DAMAGE,
        SFX_ENEMY_DIE,

        // COLLECTIBLE SFX
        SFX_COLLECT_POWERUP,

        // UI SFX
        SFX_TEXT_TYPE,
        SFX_UI_SELECT,

        // NULL OPTION
        NULL,
    }
    public enum GameMusic
    {
        MUSIC_MAIN,
        MUSIC_TOWN,
        MUSIC_OVERWORLD,
        MUSIC_COMBAT
    }
    #endregion
    public class AudioManager : MonoBehaviour
    {
        #region Mixer Groups & Audio Controllers
        [Header("Mixer Groups & Audio Controllers")]
        [SerializeField] private AudioMixerGroup mixerSFX = null;
        [SerializeField] private AudioMixerGroup mixerMusic = null;
        [SerializeField] private AudioMixer mixerMain = null;
        #endregion

        #region SFX SerializeFields
        [Header("Player SFX")]
        [SerializeField]
        private AudioClip playerRun;
        [SerializeField]
        private AudioClip combatPlayerJump;
        [SerializeField]
        private AudioClip rogueAttack;
        [SerializeField]
        private AudioClip rogueSkill;
        [SerializeField]
        private AudioClip knightAttack;
        [SerializeField]
        private AudioClip knightSkill;
        [SerializeField]
        private AudioClip mageAttack;
        [SerializeField]
        private AudioClip mageSkill;
        [SerializeField]
        private AudioClip playerTakeDamage;
        [SerializeField]
        private AudioClip playerDie;

        [Header("Enemy SFX")]
        [SerializeField]
        private AudioClip snakeAttack;
        [SerializeField]
        private AudioClip slimeAttack;
        [SerializeField]
        private AudioClip treeAttack;
        [SerializeField]
        private AudioClip enemyTakeDamage;
        [SerializeField]
        private AudioClip enemyDie;

        [Header("Collectible SFX")]
        [SerializeField]
        private AudioClip collectPowerUp;

        [Header("UI SFX")]
        [SerializeField]
        private AudioClip textType;
        [SerializeField]
        private AudioClip UISelect;

        [Header("Music")]
        [SerializeField]
        private AudioClip musicMain;
        [SerializeField]
        private AudioClip musicTown;
        [SerializeField]
        private AudioClip musicOverworld;
        [SerializeField]
        private AudioClip musicCombat;
        #endregion

        private GameObject[] SFXAudioPool;
        private const int SFXPoolSize = 500;

        private int SFXAudioPoolIndex = 0;

        private List<GameSFX> audioEffectList = new();

        private GameObject music = null;
        private AudioSource activeMusicAudioSource = null;

        public void Awake()
        {
            SFXAudioPool = new GameObject[SFXPoolSize];
            for (int i = 0; i < SFXPoolSize; ++i)
            {
                SFXAudioPool[i] = new GameObject();
                SFXAudioPool[i].name = "SFX_Pool_" + i.ToString();
                AudioSource sfxSource = SFXAudioPool[i].AddComponent<AudioSource>();
                sfxSource.maxDistance = 30f;
                sfxSource.spatialBlend = 1f;
                sfxSource.rolloffMode = AudioRolloffMode.Linear;
                if (sfxSource != null)
                {
                    sfxSource.outputAudioMixerGroup = mixerSFX;
                }

                DontDestroyOnLoad(SFXAudioPool[i]);
            }
            SFXAudioPoolIndex = 0;

            {
                music = new GameObject();
                music.name = "AUDIO_Music";
                activeMusicAudioSource = music.AddComponent<AudioSource>();
                if (null != activeMusicAudioSource) Debug.Log("Unable to add Audio Source component to goMusic");
                activeMusicAudioSource.outputAudioMixerGroup = mixerMusic;
                activeMusicAudioSource.bypassEffects = true;
                activeMusicAudioSource.bypassListenerEffects = true;
                activeMusicAudioSource.bypassReverbZones = true;
                activeMusicAudioSource.loop = true;
                activeMusicAudioSource.spatialBlend = 0.0f;
                activeMusicAudioSource.rolloffMode = AudioRolloffMode.Custom;       // Shouldn't need to set these, but just in case...
                activeMusicAudioSource.minDistance = 0.1f;
                activeMusicAudioSource.maxDistance = 100000000000.0f;

                DontDestroyOnLoad(music);
            }

            NextFrame();
        }

        public void NextFrame()
        {
            audioEffectList.Clear();
        }

        public void PlayAudioAtLocation(GameSFX intSFX, Vector3 pos, float volume = 1f, bool loop = false, bool make2D = false)
        {
            if (audioEffectList.Contains(intSFX)) return;
            else audioEffectList.Add(intSFX);

            AudioSource audioSourceSFX = SFXAudioPool[SFXAudioPoolIndex].GetComponent<AudioSource>();
            audioSourceSFX.transform.parent = null;
            DontDestroyOnLoad(audioSourceSFX);
            audioSourceSFX.Stop();

            switch (intSFX)
            {
                #region SFX switch case using ENUM
                // PLAYER SFX
                case GameSFX.SFX_PLAYER_RUN: audioSourceSFX.clip = playerRun; break;
                case GameSFX.SFX_COMBAT_PLAYER_JUMP: audioSourceSFX.clip = combatPlayerJump; break;
                case GameSFX.SFX_ROGUE_ATTACK: audioSourceSFX.clip = rogueAttack; break;
                case GameSFX.SFX_ROGUE_SKILL: audioSourceSFX.clip = rogueSkill; break;
                case GameSFX.SFX_KNIGHT_ATTACK: audioSourceSFX.clip = knightAttack; break;
                case GameSFX.SFX_KNIGHT_SKILL: audioSourceSFX.clip = knightSkill; break;
                case GameSFX.SFX_MAGE_ATTACK: audioSourceSFX.clip = mageAttack; break;
                case GameSFX.SFX_MAGE_SKILL: audioSourceSFX.clip = mageSkill; break;
                case GameSFX.SFX_PLAYER_TAKE_DAMAGE: audioSourceSFX.clip = playerTakeDamage; break;
                case GameSFX.SFX_PLAYER_DIE: audioSourceSFX.clip = enemyTakeDamage; break;

                // ENEMY SFX
                case GameSFX.SFX_SNAKE_ATTACK: audioSourceSFX.clip = snakeAttack; break;
                case GameSFX.SFX_SLIME_ATTACK: audioSourceSFX.clip = slimeAttack; break;
                case GameSFX.SFX_TREE_ATTACK: audioSourceSFX.clip = treeAttack; break;
                case GameSFX.SFX_ENEMY_TAKE_DAMAGE: audioSourceSFX.clip = enemyTakeDamage; break;
                case GameSFX.SFX_ENEMY_DIE: audioSourceSFX.clip = enemyDie; break;

                // COLLECTIBLE SFX
                case GameSFX.SFX_COLLECT_POWERUP: audioSourceSFX.clip = collectPowerUp; break;

                // UI SFX
                case GameSFX.SFX_TEXT_TYPE: audioSourceSFX.clip = textType; break;
                case GameSFX.SFX_UI_SELECT: audioSourceSFX.clip = UISelect; break;
                #endregion
            }

            SFXAudioPool[SFXAudioPoolIndex].transform.position = pos;
            audioSourceSFX.volume = volume;
            audioSourceSFX.loop = loop;
            if (make2D)
            {
                audioSourceSFX.spatialBlend = 0f;
            }
            else
            {
                audioSourceSFX.spatialBlend = 1f;
            }
            audioSourceSFX.Play();

            ++SFXAudioPoolIndex;
            if (SFXAudioPoolIndex >= SFXPoolSize) SFXAudioPoolIndex = 0;
        }

        public void PlayAudio(GameSFX intSFX)
        {
            PlayAudioAtLocation(intSFX, Camera.current.transform.position);
        }

        public void PlayMusic(GameMusic iTrackIndex, float volume = 0.2f)
        {
            if (null == activeMusicAudioSource) Debug.Log("Music Game Object is missing an audio source component");
            switch (iTrackIndex)
            {
                case GameMusic.MUSIC_MAIN: activeMusicAudioSource.clip = musicMain; break;
                case GameMusic.MUSIC_TOWN: activeMusicAudioSource.clip = musicTown; break;
                case GameMusic.MUSIC_OVERWORLD: activeMusicAudioSource.clip = musicOverworld; break;
                case GameMusic.MUSIC_COMBAT: activeMusicAudioSource.clip = musicCombat; break;
            }
            if (null == activeMusicAudioSource.clip) Debug.Log("Audio source missing for music");
            activeMusicAudioSource.volume = volume;
            activeMusicAudioSource.Play();
        }

        public void StopMusic()
        {
            if (null == activeMusicAudioSource) Debug.Log("Music Game Object is missing an audio source component");
            activeMusicAudioSource.Stop();
        }

    }
}
