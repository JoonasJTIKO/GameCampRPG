using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCampRPG
{
    public class StartMenuState : GameStateBase
    {
        public override string SceneName { get { return "StartMenu"; } }

        public override StateType Type { get { return StateType.MainMenu; } }

        public override void Activate(bool loadScene = true)
        {
            if (SceneManager.GetActiveScene().name.ToLower() != SceneName.ToLower() && loadScene)
            {
                SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
            }
            GameInstance.Instance.GetQuestCanvas().Hide();
            GameInstance.Instance.GetStartMenuCanvas().Show();
            GameInstance.Instance.GetAudioManager().PlayMusic(GameMusic.MUSIC_MAIN);
            GameInstance.Instance.GetPlayerInfo().PlayerInputs.Combat.Enable();
            GameInstance.Instance.GetPlayerInfo().PlayerInputs.UI.Enable();
        }

        public override void Deactivate(bool unloadScene = true)
        {
            GameInstance.Instance.GetPlayerInfo().PlayerInputs.Combat.Disable();
            GameInstance.Instance.GetPlayerInfo().PlayerInputs.UI.Disable();
            if (unloadScene) SceneManager.UnloadSceneAsync(SceneName);
        }

        public StartMenuState() : base() 
        {
            AddTargetState(StateType.Village);
        }
    }
}
