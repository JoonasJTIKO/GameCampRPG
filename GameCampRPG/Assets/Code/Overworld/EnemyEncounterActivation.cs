using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class EnemyEncounterActivation : MonoBehaviour
    {
        [SerializeField]
        private bool isBoss;

        private void Awake()
        {
            if (GameInstance.Instance == null) return;

            if (isBoss)
            {
                gameObject.SetActive(GameInstance.Instance.BossActive);
            }
            else
            {
                gameObject.SetActive(!GameInstance.Instance.BossActive);
            }
        }
    }
}
