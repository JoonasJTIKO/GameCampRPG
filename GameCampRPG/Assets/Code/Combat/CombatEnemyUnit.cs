using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class CombatEnemyUnit : CombatUnitBase
    {
        private EnemyGridTargeting gridTargeting;

        private Animator animator;

        private UnitShake unitShake;

        private int dazedForTurns = 0;

        public int UnitIndex;

        protected override void Awake()
        {
            base.Awake();

            gridTargeting = GetComponent<EnemyGridTargeting>();
            animator = GetComponentInChildren<Animator>();
            unitShake = GetComponent<UnitShake>();
        }

        public void Highlight(bool state)
        {
            gridTargeting.HighlightTargeted(state);

            GameInstance.Instance.GetEnemyInfoCanvas().SelectUnit(UnitIndex, state);
        }

        public void SelectAction()
        {
            if (dazedForTurns > 0) dazedForTurns--;
            if (dazedForTurns > 0) return;

            combatActions[0].QueueAction();
        }

        public void TakeDamage(int amount)
        {
            ChangeHealth(-amount);
            GameInstance.Instance.GetEnemyInfoCanvas().SetHealth(UnitIndex, Health);
            if (Health == 0)
            {
                GetComponent<EnemyGridTargeting>().UnTarget();
                SetQueuedAction(null);
                animator.SetTrigger("Death");
                return;
            }
            unitShake.Shake(0.35f);
            animator.SetTrigger("Damage");
        }

        public void Daze(int turns)
        {
            GetComponent<EnemyGridTargeting>().UnTarget();
            SetQueuedAction(null);
            dazedForTurns = turns;
        }

        public override void ChangeStats(int amount)
        {
            base.ChangeStats(amount);
            GameInstance.Instance.GetEnemyInfoCanvas().SetHealth(UnitIndex, Health);
        }
    }
}
