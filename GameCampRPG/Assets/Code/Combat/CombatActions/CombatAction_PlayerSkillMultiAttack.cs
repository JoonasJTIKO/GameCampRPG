using GameCampRPG.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class CombatAction_PlayerSkillMultiAttack : CombatActionBase
    {
        [SerializeField]
        private Transform attackSpot;

        private Vector3 attackPosition;

        [SerializeField]
        private float moveToPositionTime = 0.25f;

        private float deltaTime = 0;

        public List<CombatEnemyUnit> targetUnits = new List<CombatEnemyUnit>();

        private UnitSelection unitSelection;

        private Animator animator;

        public override void Awake()
        {
            base.Awake();

            unitSelection = FindObjectOfType<UnitSelection>();
            animator = GetComponentInChildren<Animator>();

            attackPosition = new Vector3(attackSpot.position.x, transform.position.y, attackSpot.position.z);

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
            Vector3 startPos = transform.position;

            unitSelection.SwitchTargetingMode(UnitSelection.TargetingMode.None, attackCamera: true);
            deltaTime = 0;
            while (deltaTime <= moveToPositionTime)
            {
                deltaTime += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, attackPosition, deltaTime / moveToPositionTime);
                yield return null;
            }

            if (animator != null) animator.SetTrigger("Skill");
            foreach (CombatEnemyUnit target in targetUnits)
            {
                target.TakeDamage(playerUnit.SkillStrength);
            }
            yield return new WaitForSeconds(1);

            unitSelection.SwitchTargetingMode(UnitSelection.TargetingMode.None, defaultCamera: true);
            deltaTime = 0;
            while (deltaTime <= moveToPositionTime)
            {
                deltaTime += Time.deltaTime;
                transform.position = Vector3.Lerp(attackPosition, startPos, deltaTime / moveToPositionTime);
                yield return null;
            }

            playerUnit.SetQueuedAction(null);
            targetUnits.Clear();
            Executed();
            yield return new WaitForSeconds(0.5f);
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
