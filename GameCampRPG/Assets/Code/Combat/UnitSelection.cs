using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameCampRPG
{
    public class UnitSelection : MonoBehaviour
    {
        private PlayerInputs inputs;
        private InputAction moveSelectionLeftAction, moveSelectionRightAction, 
            selectAction, goBackAction;

        [SerializeField]
        private List<CombatPlayerUnit> playerUnits;

        private CombatTurnManager turnManager;

        public enum TargetingMode
        {
            None = 0,
            PlayerUnits = 1,
            EnemyUnits = 2,
        }

        private TargetingMode currentTargetingMode = 0;

        private int currentPlayerSelection = 0;
        private int currentEnemySelection = 0;

        private int spreadTargetingAmount = 0;

        public event Action<List<CombatEnemyUnit>> OnEnemySelected;
        public event Action<CombatPlayerUnit> OnPlayerSelected;
        public event Action OnGoBack;

        private void Awake()
        {
            if (GameInstance.Instance == null) return;

            inputs = GameInstance.Instance.GetPlayerInfo().PlayerInputs;
            inputs.Combat.Enable();

            moveSelectionLeftAction = inputs.Combat.MoveSelectionLeft;
            moveSelectionRightAction = inputs.Combat.MoveSelectionRight;
            selectAction = inputs.Combat.Select;
            goBackAction = inputs.Combat.GoBack;
        }

        private void OnEnable()
        {
            if (inputs == null) return;

            moveSelectionLeftAction.performed += MoveSelectionLeft;
            moveSelectionRightAction.performed += MoveSelectionRight;
            selectAction.performed += Select;
            goBackAction.performed += GoBack;

        }

        private void OnDisable()
        {
            if (inputs == null) return;

            moveSelectionLeftAction.performed -= MoveSelectionLeft;
            moveSelectionRightAction.performed -= MoveSelectionRight;
            selectAction.performed -= Select;
            goBackAction.performed -= GoBack;
        }

        private void MoveSelectionLeft(InputAction.CallbackContext context)
        {
            if (currentTargetingMode == TargetingMode.PlayerUnits)
            {
                playerUnits[currentPlayerSelection].ActivateActionsMenu(false);

                do
                {
                    currentPlayerSelection--;
                    if (currentPlayerSelection < 0) currentPlayerSelection = playerUnits.Count - 1;
                } while (!playerUnits[currentPlayerSelection].IsAlive || !playerUnits[currentPlayerSelection].IsSelectable);

                playerUnits[currentPlayerSelection].ActivateActionsMenu(true);
            }
            else if (currentTargetingMode == TargetingMode.EnemyUnits)
            {
                do
                {
                    currentEnemySelection--;
                    if (currentEnemySelection < 0) currentEnemySelection = turnManager.EnemyUnits.Count - 1;
                } while (!turnManager.EnemyUnits[currentEnemySelection].IsAlive);

                HighlightEnemySelection(currentEnemySelection, spreadTargetingAmount);
            }
        }

        private void MoveSelectionRight(InputAction.CallbackContext context)
        {
            if (currentTargetingMode == TargetingMode.PlayerUnits)
            {
                playerUnits[currentPlayerSelection].ActivateActionsMenu(false);

                do
                {
                    currentPlayerSelection++;
                    if (currentPlayerSelection >= playerUnits.Count) currentPlayerSelection = 0;
                } while (!playerUnits[currentPlayerSelection].IsAlive || !playerUnits[currentPlayerSelection].IsSelectable);

                playerUnits[currentPlayerSelection].ActivateActionsMenu(true);
            }
            else if (currentTargetingMode == TargetingMode.EnemyUnits)
            {
                do
                {
                    currentEnemySelection++;
                    if (currentEnemySelection >= turnManager.EnemyUnits.Count) currentEnemySelection = 0;
                } while (!turnManager.EnemyUnits[currentEnemySelection].IsAlive);

                HighlightEnemySelection(currentEnemySelection, spreadTargetingAmount);
            }
        }

        private void HighlightEnemySelection(int selection, int spread)
        {
            UnHighlightAll();

            turnManager.EnemyUnits[selection].Highlight(true);

            for (int i = 1; i <= spread; i++)
            {
                if (selection + i < turnManager.EnemyUnits.Count)
                {
                    turnManager.EnemyUnits[selection + i].Highlight(true);
                }

                if (selection - i >= 0)
                {
                    turnManager.EnemyUnits[selection - i].Highlight(true);
                }
            }
        }

        private void UnHighlightAll()
        {
            foreach (CombatEnemyUnit unit in turnManager.EnemyUnits)
            {
                unit.Highlight(false);
            }
        }

        private void Select(InputAction.CallbackContext context)
        {
            UnHighlightAll();

            if (currentTargetingMode == TargetingMode.EnemyUnits)
            {
                List<CombatEnemyUnit> units = new List<CombatEnemyUnit>();
                units.Add(turnManager.EnemyUnits[currentEnemySelection]);

                for (int i = 1; i <= spreadTargetingAmount; i++)
                {
                    if (currentEnemySelection + i < turnManager.EnemyUnits.Count)
                    {
                        units.Add(turnManager.EnemyUnits[currentEnemySelection + i]);
                    }

                    if (currentEnemySelection - i >= 0)
                    {
                        units.Add(turnManager.EnemyUnits[currentEnemySelection - i]);
                    }
                }

                OnEnemySelected?.Invoke(units);
            }
            else if (currentTargetingMode == TargetingMode.PlayerUnits)
            {
                OnPlayerSelected?.Invoke(playerUnits[currentPlayerSelection]);
            }
        }

        private void GoBack(InputAction.CallbackContext context)
        {
            OnGoBack?.Invoke();
        }

        public void LockPlayerActions(bool state)
        {
            foreach (CombatPlayerUnit unit in playerUnits)
            {
                unit.LockAction = state;
            }
        }

        public void SwitchTargetingMode(TargetingMode mode, int spread = 0)
        {
            currentTargetingMode = mode;
            spreadTargetingAmount = spread;

            if (turnManager == null)
            {
                turnManager = GetComponent<CombatTurnManager>();
            }

            if (currentTargetingMode == TargetingMode.None)
            {
                turnManager.EnemyUnits[currentEnemySelection].Highlight(false);
                playerUnits[currentPlayerSelection].ActivateActionsMenu(false);
                inputs.Combat.Disable();
                return;
            }

            if (inputs != null) inputs.Combat.Enable();

            if (currentTargetingMode == TargetingMode.PlayerUnits)
            {
                playerUnits[currentPlayerSelection].ActivateActionsMenu(false);
                turnManager.EnemyUnits[currentEnemySelection].Highlight(false);
                
                while (!playerUnits[currentPlayerSelection].IsAlive || !playerUnits[currentPlayerSelection].IsSelectable)
                {
                    currentPlayerSelection++;
                    if (currentPlayerSelection >= playerUnits.Count) currentPlayerSelection = 0;
                }

                playerUnits[currentPlayerSelection].ActivateActionsMenu(true);
            }
            else if (currentTargetingMode == TargetingMode.EnemyUnits)
            {
                playerUnits[currentPlayerSelection].ActivateActionsMenu(false);

                while (!turnManager.EnemyUnits[currentEnemySelection].IsAlive)
                {
                    currentEnemySelection++;
                    if (currentEnemySelection >= turnManager.EnemyUnits.Count) currentEnemySelection = 0;
                }

                HighlightEnemySelection(currentEnemySelection, spreadTargetingAmount);
            }
        }
    }
}
