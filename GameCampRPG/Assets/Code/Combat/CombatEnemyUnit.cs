using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class CombatEnemyUnit : CombatUnitBase
    {
        private PHEnemyHealthDisplay enemyHealthDisplay;

        protected override void Awake()
        {
            base.Awake();

            enemyHealthDisplay = GetComponentInChildren<PHEnemyHealthDisplay>();
        }

        private void Start()
        {
            enemyHealthDisplay.UpdateText(Health.ToString());
        }

        public void Highlight(bool state)
        {
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
            combatActions[0].QueueAction();
        }

        public void TakeDamage(int amount)
        {
            ChangeHealth(-amount);
            enemyHealthDisplay.UpdateText(Health.ToString());
            if (Health == 0)
            {
                GetComponent<EnemyGridTargeting>().UnTarget();
            }
        }
    }
}
