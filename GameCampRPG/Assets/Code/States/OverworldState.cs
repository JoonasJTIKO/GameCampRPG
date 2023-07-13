using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

namespace GameCampRPG
{
    public class OverworldState : GameStateBase
    {
        public override string SceneName
        {
            get { return "OverworldTest"; }
        }

        public override StateType Type
        {
            get { return StateType.Overworld; }
        }

        public override void Activate(bool loadScene = true)
        {
            if (SceneManager.GetActiveScene().name.ToLower() != SceneName.ToLower() && loadScene)
            {
                SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
            }
            GameInstance.Instance.GetQuestCanvas().Show();
            GameInstance.Instance.GetSceneFadeCanvas().FadeOut();
            PlayerInputs inputs = GameInstance.Instance.GetPlayerInfo().PlayerInputs;
            inputs.Overworld.Enable();
            GameInstance.Instance.ActivateOverworldCamera(true);
        }

        public override void Deactivate()
        {
            SceneManager.UnloadSceneAsync(SceneName);
        }

        public OverworldState() : base()
        {
            AddTargetState(StateType.Village);
            AddTargetState(StateType.Combat);
        }
    }
}
