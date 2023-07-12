using GameCampRPG.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class CombatAction_PlayerSkillDoubleAct : CombatActionBase
    {
        private CombatPlayerUnit playerUnit;

        private UnitSelection unitSelection;

        private void Awake()
        {
            playerUnit = GetComponent<CombatPlayerUnit>();
            unitSelection = FindObjectOfType<UnitSelection>();

            if (GameInstance.Instance == null) return;

            cooldown -= GameInstance.Instance.GetPlayerInfo().SkillCooldownModifiers[0];
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

        public override bool QueueAction()
        {
            if (base.QueueAction())
            {
                playerUnit.IsSelectable = false;

                playerUnit.SetQueuedAction(this);
                return true;
            }

            return false;
        }

        public override bool DequeueAction()
        {
            return base.DequeueAction();
        }

        public override void Execute()
        {
            base.Execute();

            playerUnit.SetQueuedAction(null);
            StartCoroutine(Wait());
        }

        private void BeginTargetSelect()
        {
            if (onCooldown) return;

            unitSelection.LockPlayerActions(true);
            playerUnit.IsSelectable = false;
            unitSelection.OnPlayerSelected += TargetSelected;
            unitSelection.OnGoBack += StopTargetSelect;
            unitSelection.SwitchTargetingMode(UnitSelection.TargetingMode.PlayerUnits);
        }

        private void StopTargetSelect()
        {
            unitSelection.LockPlayerActions(false);
            playerUnit.IsSelectable = true;
            unitSelection.OnPlayerSelected -= TargetSelected;
            unitSelection.OnGoBack -= StopTargetSelect;
            unitSelection.SwitchTargetingMode(UnitSelection.TargetingMode.PlayerUnits);
        }

        private void TargetSelected(CombatPlayerUnit unit)
        {
            unitSelection.LockPlayerActions(false);
            unitSelection.OnPlayerSelected -= TargetSelected;
            unitSelection.OnGoBack -= StopTargetSelect;
            unit.DoExtraAction = true;
            QueueAction();
            unitSelection.SwitchTargetingMode(UnitSelection.TargetingMode.PlayerUnits);
        }

        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(0.5f);
            Executed();
        }
    }
}
