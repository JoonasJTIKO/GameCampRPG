using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class OverworldTransition : MonoBehaviour
    {
        [SerializeField]
        private List<EnemyData> enemies;

        [SerializeField]
        private List<Item> drops;

        [SerializeField]
        private StateType targetState;

        [SerializeField]
        private bool unloadCurrentScene = true;

        private void OnTriggerEnter(Collider other)
        {
            gameObject.GetComponent<Collider>().enabled = false;
            PlayerInputs inputs = GameInstance.Instance.GetPlayerInfo().PlayerInputs;
            inputs.Overworld.Disable();
            GameInstance.Instance.GetCombatInfo().Enemies = enemies;
            GameInstance.Instance.GetCombatInfo().Drops = drops;
            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            GameInstance.Instance.GetSceneFadeCanvas().FadeIn();
            yield return new WaitForSeconds(1.1f);
            GameInstance.Instance.ActivateOverworldCamera(false);
            gameObject.SetActive(false);
            GameInstance.Instance.GetGameStateManager().Go(targetState, unloadCurrentScene);
        }
    }
}
