using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class CombatEnemyUnit : CombatUnitBase
    {
        private EnemyGridTargeting gridTargeting;

        private int dazedForTurns = 0;

        public int UnitIndex;

        protected override void Awake()
        {
            base.Awake();

            gridTargeting = GetComponent<EnemyGridTargeting>();
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
            }
        }

        public void Daze(int turns)
        {
            GetComponent<EnemyGridTargeting>().UnTarget();
            SetQueuedAction(null);
            dazedForTurns = turns;
        }
    }
}
