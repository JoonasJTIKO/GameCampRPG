using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class CombatAction_EnemyAttack : CombatActionBase
    {
        [SerializeField]
        private EnemyGridTargeting.TargetingType targetingType;

        private CombatEnemyUnit enemyUnit;

        private EnemyGridTargeting targeting;

        private GridVisual grid;

        public override void Awake()
        {
            base.Awake();

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
                switch (targetingType)
                {
                    case EnemyGridTargeting.TargetingType.Line:
                        targeting.Target(Random.Range(0, grid.Size), targetingType, (EnemyGridTargeting.TargetingDirection)Random.Range(0, 4));
                        break;
                    case EnemyGridTargeting.TargetingType.Square:
                        targeting.Target(0, targetingType, startX: Random.Range(0, grid.Size - 1), startY: Random.Range(0, grid.Size - 1));
                        break;
                    case EnemyGridTargeting.TargetingType.Cross:
                        targeting.Target(0, targetingType, startX: Random.Range(1, grid.Size - 1), startY: Random.Range(1, grid.Size - 1));
                        break;
                }
                enemyUnit.SetQueuedAction(this);
                return true;
            }
            return false;
        }

        public override void Execute()
        {
            base.Execute();

            targeting.AttackTargeted(enemyUnit.AttackStrength);
            enemyUnit.SetQueuedAction(null);
        }
    }
}
