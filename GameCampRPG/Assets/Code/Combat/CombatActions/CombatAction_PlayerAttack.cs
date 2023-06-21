using GameCampRPG.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class CombatAction_PlayerAttack : CombatActionBase
    {
        private CombatEnemyUnit targetUnit = null;

        private CombatPlayerUnit playerUnit;

        private UnitSelection unitSelection;

        private void Awake()
        {
            playerUnit = GetComponent<CombatPlayerUnit>();
            unitSelection = FindObjectOfType<UnitSelection>();
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
            targetUnit.TakeDamage(playerUnit.AttackStrength);
            yield return new WaitForSeconds(1);
            Executed();
            yield return null;
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
            playerUnit.SetQueuedAction(null);
            targetUnit = null;
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
