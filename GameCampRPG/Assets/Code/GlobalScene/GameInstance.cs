using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCampRPG.UI;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using GameCampRPG.Quests;

namespace GameCampRPG
{
    public class GameInstance : MonoBehaviour
    {
        [SerializeField]
        private SceneFadeCanvas UI_SceneFadeCanvas;

        [SerializeField]
        private Dialogue UI_DialogueCanvas;

        [SerializeField]
        private PlayerCombatCanvas UI_PlayerCombatCanvas;

        [SerializeField]
        private PauseMenuCanvas UI_PauseMenuCanvas;

        [SerializeField]
        private QuestCanvas UI_QuestCanvas;

        private Camera overworldCamera;

        private GameStateManager gameStateManager;

        private PlayerInfo playerInfo;

        private CombatInfo combatInfo;

        private QuestManager questManager;

        private static GameInstance instance;

        public static GameInstance Instance
        {
            get
            {
                if (!instance)
                {
                    instance = FindObjectOfType<GameInstance>();
                }
                return instance;
            }
        }

        public bool UsingController
        {
            get;
            private set;
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = GetComponent<GameInstance>();
            }
            UsingController = Gamepad.all.Count > 0;
            gameStateManager = GetComponent<GameStateManager>();
            playerInfo = GetComponent<PlayerInfo>();
            combatInfo = GetComponent<CombatInfo>();
            questManager = GetComponent<QuestManager>();
        }

        public void SetOverworldCamera(Camera camera)
        {
            overworldCamera = camera;
        }

        public void ActivateOverworldCamera(bool state)
        {
            if (overworldCamera == null) return;

            overworldCamera.enabled = state;
            overworldCamera.GetComponent<AudioListener>().enabled = state;
        }

        public GameStateManager GetGameStateManager()
        {
            return gameStateManager;
        }

        public PlayerInfo GetPlayerInfo()
        {
            return playerInfo;
        }

        public CombatInfo GetCombatInfo()
        {
            return combatInfo;
        }

        public SceneFadeCanvas GetSceneFadeCanvas()
        {
            return UI_SceneFadeCanvas;
        }

        public Dialogue GetDialogueCanvas()
        {
            return UI_DialogueCanvas;
        }

        public PlayerCombatCanvas GetPlayerCombatCanvas()
        {
            return UI_PlayerCombatCanvas;
        }

        public PauseMenuCanvas GetPauseMenuCanvas()
        {
            return UI_PauseMenuCanvas;
        }

        public QuestCanvas GetQuestCanvas()
        {
            return UI_QuestCanvas;
        }

        public QuestManager GetQuestManager()
        {
            return questManager;
        }
    }
}
