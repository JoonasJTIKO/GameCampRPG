using GameCampRPG.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class CombatAction_PlayerSkillMultiAttack : CombatActionBase
    {
        public List<CombatEnemyUnit> targetUnits = new List<CombatEnemyUnit>();

        private CombatPlayerUnit playerUnit;

        private UnitSelection unitSelection;

        private void Awake()
        {
            playerUnit = GetComponent<CombatPlayerUnit>();
            unitSelection = FindObjectOfType<UnitSelection>();

            if (GameInstance.Instance == null) return;

            cooldown -= GameInstance.Instance.GetPlayerInfo().SkillCooldownModifiers[1];
        }

        private void TargetSelected(List<CombatEnemyUnit> targets)
        {
            unitSelection.OnEnemySelected -= TargetSelected;
            unitSelection.OnGoBack -= StopTargetSelect;
            List<CombatEnemyUnit> units = new List<CombatEnemyUnit>();
            foreach (CombatEnemyUnit target in targets)
            {
                units.Add(target);
            }
            QueueAction(units);
            unitSelection.SwitchTargetingMode(UnitSelection.TargetingMode.PlayerUnits);
        }

        private IEnumerator AttackEnemies()
        {
            foreach (CombatEnemyUnit target in targetUnits)
            {
                target.TakeDamage(playerUnit.SkillStrength);
            }
            yield return new WaitForSeconds(1);
            Executed();
            yield return null;
        }

        public bool QueueAction(List<CombatEnemyUnit> targets)
        {
            if (QueueAction())
            {
                playerUnit.IsSelectable = false;

                extraAction = playerUnit.SetQueuedAction(this);
                targetUnits = targets;
                return true;
            }
            return false;
        }

        public override bool DequeueAction()
        {
            targetUnits.Clear();
            return base.DequeueAction();
        }

        public override void Execute()
        {
            base.Execute();

            StartCoroutine(AttackEnemies());
            playerUnit.SetQueuedAction(null);
            targetUnits.Clear();
        }

        public override void BeginListening()
        {
            base.BeginListening();

            PlayerCombatCanvas.OnSkillPressed += BeginTargetSelect;
        }

        public override void StopListening()
        {
            base.StopListening();

            PlayerCombatCanvas.OnSkillPressed -= BeginTargetSelect;
        }

        public void BeginTargetSelect()
        {
            if (onCooldown) return;

            unitSelection.SwitchTargetingMode(UnitSelection.TargetingMode.EnemyUnits, playerUnit.SkillStrength);
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
