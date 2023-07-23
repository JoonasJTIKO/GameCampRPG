using GameCampRPG.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GameCampRPG
{
    public class CombatAction_PlayerMove : CombatActionBase
    {
        private int maxMoveDistance = 1;

        private GridNavigation gridNavigation;
        private UnitSelection unitSelection;
        private CombatPlayerMoving playerMoving;
        private CombatPlayerBuffManager playerBuffManager;
        private Animator animator;

        private int targetX, targetY;

        public override void Awake()
        {
            base.Awake();

            gridNavigation = FindObjectOfType<GridNavigation>();
            unitSelection = FindObjectOfType<UnitSelection>();
            playerMoving = GetComponent<CombatPlayerMoving>();
            playerBuffManager = GetComponent<CombatPlayerBuffManager>();
            animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            playerMoving.Moved += Executed;
            playerBuffManager.MovementBuffActivated += ChangeMoveDistance;
        }

        private void OnDisable()
        {
            playerMoving.Moved -= Executed;
            playerBuffManager.MovementBuffActivated -= ChangeMoveDistance;
        }

        public override bool QueueAction()
        {
            if (base.QueueAction())
            {
                targetX = gridNavigation.SelectedX;
                targetY = gridNavigation.SelectedY;

                extraAction = playerUnit.SetQueuedAction(this);
                if (!extraAction) playerUnit.IsSelectable = false;

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
            animator.SetTrigger("Jump");
            playerMoving.MoveToPosition(targetX, targetY);
        }

        public override void BeginListening()
        {
            base.BeginListening();

            PlayerCombatCanvas.OnMovePressed += SelectMoveTarget;
        }

        public override void StopListening()
        {
            base.StopListening();

            PlayerCombatCanvas.OnMovePressed -= SelectMoveTarget;
        }

        private void SelectMoveTarget()
        {
            unitSelection.SwitchTargetingMode(UnitSelection.TargetingMode.None);
            unitSelection.OnGoBack += CancelSelect;
            gridNavigation.OnSelected += OnSelected;
            gridNavigation.StartNavigation(playerMoving.GridPositionX, playerMoving.GridPositionY, maxMoveDistance);
        }

        private void CancelSelect()
        {
            unitSelection.OnGoBack -= CancelSelect;
            gridNavigation.StopNavigation();
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

        private void ChangeMoveDistance(bool input)
        {
            if (input) maxMoveDistance = 2;
            else maxMoveDistance = 1;
        }
    }
}
