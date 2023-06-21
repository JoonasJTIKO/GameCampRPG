using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class CombatAction_EnemyAttack : CombatActionBase
    {
        private CombatEnemyUnit enemyUnit;

        private EnemyGridTargeting targeting;

        private GridVisual grid;

        private void Awake()
        {
            enemyUnit = GetComponent<CombatEnemyUnit>();
            targeting = GetComponent<EnemyGridTargeting>();

            grid = FindObjectOfType<GridVisual>();
        }

        private void OnEnable()
        {
            targeting.OnAttackFinished += Executed;
        }

        private void OnDisable()
        {
            targeting.OnAttackFinished -= Executed;
        }

        public override bool QueueAction()
        {
            if (base.QueueAction())
            {
                targeting.Target(Random.Range(0, grid.Size), EnemyGridTargeting.TargetingType.Line, (EnemyGridTargeting.TargetingDirection)Random.Range(0, 4));
                enemyUnit.SetQueuedAction(this);
                return true;
            }
            return false;
        }

        public override void Execute()
        {
            base.Execute();
            
            targeting.AttackTargeted();
            enemyUnit.SetQueuedAction(null);
        }
    }
}
