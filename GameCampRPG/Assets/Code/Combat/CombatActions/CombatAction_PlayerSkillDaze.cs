using GameCampRPG.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class CombatAction_PlayerSkillDaze : CombatActionBase
    {
        [SerializeField]
        private Transform attackSpot;

        private Vector3 attackPosition;

        [SerializeField]
        private float moveToPositionTime = 0.25f;

        private float deltaTime = 0;

        private UnitSelection unitSelection;

        private CombatPlayerBuffManager playerBuffManager;

        private int buffedStrength = 0;

        private Animator animator;

        public override void Awake()
        {
            base.Awake();

            unitSelection = FindObjectOfType<UnitSelection>();
            playerBuffManager = GetComponent<CombatPlayerBuffManager>();
            animator = GetComponentInChildren<Animator>();

            attackPosition = new Vector3(attackSpot.position.x, transform.position.y, attackSpot.position.z);

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

            StartCoroutine(Daze(units));
        }

        private IEnumerator Daze(List<CombatEnemyUnit> units)
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

            units[0].Daze(GameInstance.Instance.GetPlayerInfo().SkillStrengths[0] + buffedStrength);
            if (animator != null) animator.SetTrigger("Skill");
            yield return new WaitForSeconds(1);

            unitSelection.SwitchTargetingMode(UnitSelection.TargetingMode.None, defaultCamera: true);
            deltaTime = 0;
            while (deltaTime <= moveToPositionTime)
            {
                deltaTime += Time.deltaTime;
                transform.position = Vector3.Lerp(attackPosition, startPos, deltaTime / moveToPositionTime);
                yield return null;
            }

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
