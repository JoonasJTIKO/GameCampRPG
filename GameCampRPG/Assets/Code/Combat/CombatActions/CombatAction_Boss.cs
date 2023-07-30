using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameCampRPG.EnemyGridTargeting;

namespace GameCampRPG
{
    public class CombatAction_Boss : CombatActionBase
    {
        private CombatEnemyUnit enemyUnit;

        private EnemyGridTargeting targeting;

        private GridVisual grid;

        private Animator animator;

        private EnemyGridTargeting.TargetingType targetingType;

        public override void Awake()
        {
            base.Awake();

            enemyUnit = GetComponent<CombatEnemyUnit>();
            targeting = GetComponent<EnemyGridTargeting>();

            grid = FindObjectOfType<GridVisual>();

            animator = GetComponentInChildren<Animator>();
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
            if (enemyUnit.Health < (enemyUnit.MaxHealth * (1 / 3)))
            {
                int random = Random.Range(1, 11);
                if (random == 10) targetingType = TargetingType.FullGrid;
                else if (random < 10 && random > 5) targetingType = TargetingType.BigCross;
                else targetingType = TargetingType.BigLine;
            }
            else if (enemyUnit.Health < (enemyUnit.MaxHealth * (2 / 3)))
            {
                int random = Random.Range(1, 3);
                if (random == 1) targetingType = TargetingType.BigCross;
                else targetingType = TargetingType.BigLine;
            }
            else
            {
                int random = Random.Range(1, 4);
                if (random == 1) targetingType = TargetingType.BigLine;
                else targetingType = TargetingType.BigSquare;
            }

            if (base.QueueAction())
            {
                switch (targetingType)
                {
                    case EnemyGridTargeting.TargetingType.BigLine:
                        targeting.Target(Random.Range(1, grid.Size - 1), enemyUnit.AttackStrength, targetingType, (EnemyGridTargeting.TargetingDirection)Random.Range(0, 4));
                        break;
                    case EnemyGridTargeting.TargetingType.BigSquare:
                        targeting.Target(0, enemyUnit.AttackStrength, targetingType, startX: Random.Range(1, grid.Size - 1), startY: Random.Range(1, grid.Size - 1));
                        break;
                    case EnemyGridTargeting.TargetingType.BigCross:
                        targeting.Target(0, enemyUnit.AttackStrength, targetingType, startX: Random.Range(grid.Size - 2, grid.Size - 2), startY: Random.Range(grid.Size - 2, grid.Size - 2));
                        break;
                    case TargetingType.FullGrid:
                        if (animator != null) animator.SetTrigger("Cast");
                        targeting.Target(0, enemyUnit.AttackStrength, targetingType);
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

            if (animator != null && targetingType == TargetingType.BigLine) animator.SetTrigger("Attack");
            else if (animator != null) animator.SetTrigger("AttackBig");
            targeting.AttackTargeted(enemyUnit.AttackStrength);
            enemyUnit.SetQueuedAction(null);
        }
    }
}
