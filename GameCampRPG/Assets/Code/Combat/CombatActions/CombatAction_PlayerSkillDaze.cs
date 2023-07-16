using GameCampRPG.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class CombatAction_PlayerSkillDaze : CombatActionBase
    {
        private UnitSelection unitSelection;

        private CombatPlayerBuffManager playerBuffManager;

        private int buffedStrength = 0;

        public override void Awake()
        {
            base.Awake();

            unitSelection = FindObjectOfType<UnitSelection>();
            playerBuffManager = GetComponent<CombatPlayerBuffManager>();

            if (GameInstance.Instance == null) return;

            cooldown -= GameInstance.Instance.GetPlayerInfo().SkillCooldownModifiers[0];
        }

        private void OnEnable()
        {
            playerBuffManager.SkillBuffActivated += BoostEffect;
        }

        private void OnDisable()
        {
            playerBuffManager.SkillBuffActivated -= BoostEffect;
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

        private void TargetSelected(List<CombatEnemyUnit> units)
        {
            unitSelection.OnEnemySelected -= TargetSelected;
            unitSelection.OnGoBack -= StopTargetSelect;

            units[0].Daze(GameInstance.Instance.GetPlayerInfo().SkillStrengths[0] + buffedStrength);

            QueueAction();
            unitSelection.SwitchTargetingMode(UnitSelection.TargetingMode.PlayerUnits);
        }

        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(0.5f);
            Executed();
        }

        private void BoostEffect(bool input)
        {
            if (input)
            {
                buffedStrength = 1;
            }
            else
            {
                buffedStrength = 0;
            }
        }
    }
}
