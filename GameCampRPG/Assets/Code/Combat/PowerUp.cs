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

        [SerializeField]
        private MeshRenderer[] meshRenderers;

        private new Collider collider;

        private PowerUpSpawning powerUpSpawning;

        private SineBounce sineBounce;

        private void Awake()
        {
            collider = GetComponent<Collider>();
            sineBounce = GetComponent<SineBounce>();
            Deactivate();

            powerUpSpawning = FindObjectOfType<PowerUpSpawning>();
        }

        public void Activate()
        {
            sineBounce.origin = transform.position;
            foreach (MeshRenderer renderer in meshRenderers)
            {
                renderer.enabled = true;
            }
            collider.enabled = true;

        }

        private void Deactivate()
        {
            foreach (MeshRenderer renderer in meshRenderers)
            {
                renderer.enabled = false;
            }
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

            GameInstance.Instance.GetAudioManager().PlayAudioAtLocation(GameSFX.SFX_COLLECT_POWERUP, other.transform.position);
            powerUpSpawning.Spawned = null;
            Deactivate();
        }
    }
}
