using GameCampRPG.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class CombatAction_PlayerAttack : CombatActionBase
    {
        [SerializeField]
        private Transform attackSpot;

        [SerializeField]
        private float moveToPositionTime = 0.25f;

        private float deltaTime = 0;

        private CombatEnemyUnit targetUnit = null;

        private UnitSelection unitSelection;

        private Animator animator;

        public override void Awake()
        {
            base.Awake();

            unitSelection = FindObjectOfType<UnitSelection>();
            animator = GetComponentInChildren<Animator>();
        }

        private void TargetSelected(List<CombatEnemyUnit> targets)
        {
            unitSelection.OnEnemySelected -= TargetSelected;
            unitSelection.OnGoBack -= StopTargetSelect;
            QueueAction(targets[0]);
            unitSelection.SwitchTargetingMode(UnitSelection.TargetingMode.PlayerUnits);
        }

        private IEnumerator AttackEnemy()
        {
            Vector3 startPos = transform.position;

            unitSelection.SwitchTargetingMode(UnitSelection.TargetingMode.None, attackCamera: true);
            deltaTime = 0;
            while (deltaTime <= moveToPositionTime)
            {
                deltaTime += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, attackSpot.position, deltaTime / moveToPositionTime);
                yield return null;
            }
            if (animator != null) animator.SetTrigger("Attack");
            targetUnit.TakeDamage(playerUnit.AttackStrength);
            yield return new WaitForSeconds(1);

            unitSelection.SwitchTargetingMode(UnitSelection.TargetingMode.None, defaultCamera: true);
            deltaTime = 0;
            while (deltaTime <= moveToPositionTime)
            {
                deltaTime += Time.deltaTime;
                transform.position = Vector3.Lerp(attackSpot.position, startPos, deltaTime / moveToPositionTime);
                yield return null;
            }

            playerUnit.SetQueuedAction(null);
            targetUnit = null;
            Executed();
            yield return new WaitForSeconds(0.5f);
        }

        public bool QueueAction(CombatEnemyUnit target)
        {
            if (QueueAction())
            {
                extraAction = playerUnit.SetQueuedAction(this);
                if (!extraAction) playerUnit.IsSelectable = false;
                targetUnit = target;
                return true;
            }
            return false;
        }

        public override bool DequeueAction()
        {
            targetUnit = null;
            return base.DequeueAction();
        }

        public override void Execute()
        {
            base.Execute();

            StartCoroutine(AttackEnemy());
        }

        public override void BeginListening()
        {
            base.BeginListening();

            PlayerCombatCanvas.OnAttackPressed += BeginTargetSelect;
        }

        public override void StopListening()
        {
            base.StopListening();

            PlayerCombatCanvas.OnAttackPressed -= BeginTargetSelect;
        }

        public void BeginTargetSelect()
        {
            unitSelection.SwitchTargetingMode(UnitSelection.TargetingMode.EnemyUnits);
            unitSelection.OnEnemySelected += TargetSelected;
            unitSelection.OnGoBack += StopTargetSelect;
        }

        private void StopTargetSelect()
        {
            unitSelection.SwitchTargetingMode(UnitSelection.TargetingMode.PlayerUnits);
            unitSelection.OnEnemySelected -= TargetSelected;
            unitSelection.OnGoBack -= StopTargetSelect;
        }
    }
}
