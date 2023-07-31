using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCampRPG
{
    public class VillageState : GameStateBase
    {
        public override string SceneName
        {
            get { return "NPCTest"; }
        }

        public override StateType Type
        {
            get { return StateType.Village; }
        }

        public override void Activate(bool loadScene = true)
        {
            if (SceneManager.GetActiveScene().name.ToLower() != SceneName.ToLower() && loadScene)
            {
                SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
            }
            PlayerInputs inputs = GameInstance.Instance.GetPlayerInfo().PlayerInputs;
            GameInstance.Instance.GetStartMenuCanvas().Hide();
            GameInstance.Instance.GetSceneFadeCanvas().FadeOut();
            GameInstance.Instance.GetAudioManager().PlayMusic(GameMusic.MUSIC_TOWN);
            inputs.Overworld.Enable();
        }

        public override void Deactivate(bool unloadScene = true)
        {
            if (unloadScene) SceneManager.UnloadSceneAsync(SceneName);
        }

        public VillageState() : base()
        {
            AddTargetState(StateType.Overworld);
        }
    }
}
