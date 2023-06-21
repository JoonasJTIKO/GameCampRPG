using GameCampRPG.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GameCampRPG
{
    public class CombatAction_PlayerMove : CombatActionBase
    {
        [SerializeField]
        private int maxMoveDistance = 1;

        private GridNavigation gridNavigation;
        private UnitSelection unitSelection;
        private CombatPlayerMoving playerMoving;
        private CombatPlayerUnit playerUnit;

        private int targetX, targetY;

        private void Awake()
        {
            gridNavigation = FindObjectOfType<GridNavigation>();
            unitSelection = FindObjectOfType<UnitSelection>();
            playerMoving = GetComponent<CombatPlayerMoving>();
            playerUnit = GetComponent<CombatPlayerUnit>();
        }

        private void OnEnable()
        {
            playerMoving.Moved += Executed;
        }

        private void OnDisable()
        {
            playerMoving.Moved -= Executed;
        }

        public override bool QueueAction()
        {
            if (base.QueueAction())
            {
                targetX = gridNavigation.SelectedX;
                targetY = gridNavigation.SelectedY;

                extraAction = playerUnit.SetQueuedAction(this);

                return true;
            }

            return false;
        }

        public override bool DequeueAction()
        {
            playerUnit.SetQueuedAction(null);
            return base.DequeueAction();
        }

        public override void Execute()
        {
            base.Execute();

            playerUnit.SetQueuedAction(null);
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
    }
}
