using GameCampRPG.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class CombatAction_PlayerSkillHeal : CombatActionBase
    {
        private GridNavigation gridNavigation;
        private UnitSelection unitSelection;
        private CombatPlayerMoving playerMoving;
        private CombatPlayerBuffManager playerBuffManager;
        private GridVisual grid;

        private int targetX, targetY;

        private int healAmount = 1;

        private GridNavigation.TargetingShape targetingShape = GridNavigation.TargetingShape.Cross;

        public override void Awake()
        {
            base.Awake();

            gridNavigation = FindObjectOfType<GridNavigation>();
            unitSelection = FindObjectOfType<UnitSelection>();
            playerMoving = GetComponent<CombatPlayerMoving>();
            playerBuffManager = GetComponent<CombatPlayerBuffManager>();
            grid = FindObjectOfType<GridVisual>();

            if (GameInstance.Instance == null) return;

            cooldown -= GameInstance.Instance.GetPlayerInfo().SkillCooldownModifiers[0];
        }

        private void OnEnable()
        {
            playerBuffManager.SkillBuffActivated += ChangeShape;
        }

        private void OnDisable()
        {
            playerBuffManager.SkillBuffActivated -= ChangeShape;
        }

        public override bool QueueAction()
        {
            if (base.QueueAction())
            {
                targetX = gridNavigation.SelectedX;
                targetY = gridNavigation.SelectedY;

                playerUnit.IsSelectable = false;

                extraAction = playerUnit.SetQueuedAction(this);

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
            StartCoroutine(HealTargetedArea());
        }

        public override void BeginListening()
        {
            base.BeginListening();

            PlayerCombatCanvas.OnSkillPressed += SelectHealTarget;
        }

        public override void StopListening()
        {
            base.StopListening();

            PlayerCombatCanvas.OnSkillPressed -= SelectHealTarget;
        }

        private IEnumerator HealTargetedArea()
        {
            switch (targetingShape)
            {
                case GridNavigation.TargetingShape.Single:
                    Heal(targetX, targetY);
                    break;
                case GridNavigation.TargetingShape.Cross:
                    Heal(targetX, targetY);
                    Heal(targetX + 1, targetY);
                    Heal(targetX - 1, targetY);
                    Heal(targetX, targetY + 1);
                    Heal(targetX, targetY - 1);
                    break;
                case GridNavigation.TargetingShape.Square:
                    Heal(targetX, targetY);
                    Heal(targetX + 1, targetY);
                    Heal(targetX - 1, targetY);
                    Heal(targetX, targetY + 1);
                    Heal(targetX, targetY - 1);
                    Heal(targetX + 1, targetY + 1);
                    Heal(targetX + 1, targetY - 1);
                    Heal(targetX - 1, targetY + 1);
                    Heal(targetX - 1, targetY - 1);
                    break;
            }
            yield return new WaitForSeconds(1);
            Executed();
        }

        private void Heal(int x, int y)
        {
            GameObject go = grid.CheckForPlayer(x, y);
            if (go == null) return;

            CombatPlayerUnit unit = go.GetComponent<CombatPlayerUnit>();
            unit.ChangeHealth(healAmount + playerUnit.SkillStrength);
            Debug.Log("Healing " + unit);
        }

        private void SelectHealTarget()
        {
            if (onCooldown) return;

            unitSelection.SwitchTargetingMode(UnitSelection.TargetingMode.None);
            unitSelection.OnGoBack += CancelSelect;
            gridNavigation.OnSelected += OnSelected;
            gridNavigation.StartNavigation(playerMoving.GridPositionX, playerMoving.GridPositionY, 99, targetingShape, true);
        }

        private void CancelSelect()
        {
            unitSelection.OnGoBack -= CancelSelect;
            gridNavigation.StopNavigation(targetingShape);
            gridNavigation.OnSelected -= OnSelected;
            unitSelection.SwitchTargetingMode(UnitSelection.TargetingMode.PlayerUnits);
        }

        private void OnSelected()
        {
            QueueAction();
            gridNavigation.OnSelected -= OnSelected;
            unitSelection.OnGoBack -= CancelSelect;
            unitSelection.SwitchTargetingMode(UnitSelection.TargetingMode.PlayerUnits);
        }

        private void ChangeShape(bool input)
        {
            if (input)
            {
                targetingShape = GridNavigation.TargetingShape.Square;
            }
            else
            {
                targetingShape = GridNavigation.TargetingShape.Cross;
            }
        }
    }
}
