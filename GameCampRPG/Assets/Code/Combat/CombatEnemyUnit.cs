using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class CombatEnemyUnit : CombatUnitBase
    {
        private PHEnemyHealthDisplay enemyHealthDisplay;

        private EnemyGridTargeting gridTargeting;

        private int dazedForTurns = 0;

        protected override void Awake()
        {
            base.Awake();

            enemyHealthDisplay = GetComponentInChildren<PHEnemyHealthDisplay>();
            gridTargeting = GetComponent<EnemyGridTargeting>();
        }

        private void Start()
        {
            enemyHealthDisplay.UpdateText(Health.ToString());
        }

        public void Highlight(bool state)
        {
            gridTargeting.HighlightTargeted(state);

            if (state)
            {
                gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
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
            enemyHealthDisplay.UpdateText(Health.ToString());
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
