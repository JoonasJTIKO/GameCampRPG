using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCampRPG
{
    public class CombatState : GameStateBase
    {
        public override string SceneName
        {
            get { return "CombatTest"; }
        }

        public override StateType Type
        {
            get { return StateType.Combat; }
        }

        public override void Activate(bool loadScene = true)
        {
            if (SceneManager.GetActiveScene().name.ToLower() != SceneName.ToLower() && loadScene)
            {
                SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
            }
            GameInstance.Instance.GetSceneFadeCanvas().FadeOut();
        }

        public override void Deactivate()
        {
            SceneManager.UnloadSceneAsync(SceneName);
        }

        public CombatState() : base()
        {
            AddTargetState(StateType.Overworld);
        }
    }
}
