using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public enum PowerUpType
    {
        None,
        Attack,
        Skill,
        Movement
    }

    public class PowerUp : MonoBehaviour
    {
        [SerializeField]
        private PowerUpType powerUpType;

        private MeshRenderer meshRenderer;

        private new Collider collider;

        private PowerUpSpawning powerUpSpawning;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            collider = GetComponent<Collider>();
            Deactivate();

            powerUpSpawning = FindObjectOfType<PowerUpSpawning>();
        }

        public void Activate()
        {
            meshRenderer.enabled = true;
            collider.enabled = true;
        }

        private void Deactivate()
        {
            meshRenderer.enabled = false;
            collider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            CombatPlayerBuffManager buffManager = other.GetComponent<CombatPlayerBuffManager>();

            if (buffManager == null) return;

            switch (powerUpType)
            {
                case PowerUpType.Attack:
                    buffManager.ActivateAttackBuff(true);
                    break;
                case PowerUpType.Skill:
                    buffManager.ActivateSkillBuff(true);
                    break;
                case PowerUpType.Movement:
                    buffManager.ActivateMovementBuff(true);
                    break;
            }

            powerUpSpawning.Spawned = null;
            Deactivate();
        }
    }
}
