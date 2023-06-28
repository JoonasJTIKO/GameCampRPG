using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameCampRPG
{
    public class GridNavigation : MonoBehaviour
    {
        private GridVisual gridVisual;

        private PlayerInputs playerInputs;
        private InputAction moveAction, selectAction;

        private int startX, currentX, currentY, startY, maxRange;

        private bool navigating = false, canMove = true, allowOccupiedSelect;

        public event Action OnSelected;

        public int SelectedX
        {
            get;
            private set;
        }

        public int SelectedY
        {
            get;
            private set;
        }

        public enum TargetingShape
        {
            Single = 0,
            Cross = 1,
            Square = 2,
        }

        private TargetingShape targetingShape;

        private void Awake()
        {
            if (GameInstance.Instance == null) return;

            gridVisual = GetComponent<GridVisual>();

            playerInputs = GameInstance.Instance.GetPlayerInfo().PlayerInputs;

            moveAction = playerInputs.Combat.Navigate;
            selectAction = playerInputs.Combat.Select;
        }

        private void Update()
        {
            if (!navigating) return;

            if (moveAction.phase == InputActionPhase.Waiting)
            {
                canMove = true;
            }

            if (canMove && moveAction.phase == InputActionPhase.Started)
            {
                MoveSelection(moveAction.ReadValue<Vector2>(), targetingShape);
                canMove = false;
            }
        }

        public void StartNavigation(int startX, int startY, int maxRange, TargetingShape shape = TargetingShape.Single, bool allowOccupied = false)
        {
            this.startX = startX;
            currentX = startX;
            this.startY = startY;
            currentY = startY;
            this.maxRange = maxRange;
            targetingShape = shape;
            allowOccupiedSelect = allowOccupied;

            HighlightShape(currentX, currentY, true, shape);

            navigating = true;
            playerInputs.Combat.Enable();
            selectAction.performed += Select;
        }

        public void StopNavigation(TargetingShape shape = TargetingShape.Single)
        {
            playerInputs.Combat.Disable();

            HighlightShape(currentX, currentY, false, shape);

            navigating = false;
            selectAction.performed -= Select;
        }

        private void HighlightShape(int x, int y, bool state, TargetingShape shape)
        {
            switch (shape)
            {
                case TargetingShape.Single:
                    gridVisual.HighlightNode(x, y, state);
                    break;
                case TargetingShape.Cross:
                    gridVisual.HighlightNode(x, y, state);
                    gridVisual.HighlightNode(x + 1, y, state);
                    gridVisual.HighlightNode(x - 1, y, state);
                    gridVisual.HighlightNode(x, y + 1, state);
                    gridVisual.HighlightNode(x, y - 1, state);
                    break;
                case TargetingShape.Square:
                    gridVisual.HighlightNode(x, y, state);
                    gridVisual.HighlightNode(x + 1, y, state);
                    gridVisual.HighlightNode(x - 1, y, state);
                    gridVisual.HighlightNode(x, y + 1, state);
                    gridVisual.HighlightNode(x, y - 1, state);
                    gridVisual.HighlightNode(x + 1, y + 1, state);
                    gridVisual.HighlightNode(x + 1, y - 1, state);
                    gridVisual.HighlightNode(x - 1, y + 1, state);
                    gridVisual.HighlightNode(x - 1, y - 1, state);
                    break;
            }
        }

        private bool MoveSelection(Vector2 input, TargetingShape shape)
        {
            int newX = (int)(currentX + input.y);
            int newY = (int)(currentY - input.x);

            if (Mathf.Abs(newX - startX) > maxRange || Mathf.Abs(newY - startY) > maxRange
                || newX < 0 || newX >= gridVisual.Size || newY < 0 || newY >= gridVisual.Size)
            {
                return false;
            }

            HighlightShape(currentX, currentY, false, shape);
            HighlightShape(newX, newY, true, shape);

            currentX = newX;
            currentY = newY;

            return true;
        }

        private void Select(InputAction.CallbackContext context)
        {
            if (gridVisual.MoveToNode(currentX, currentY) || allowOccupiedSelect)
            {
                playerInputs.Combat.Disable();

                SelectedX = currentX;
                SelectedY = currentY;

                HighlightShape(currentX, currentY, false, targetingShape);

                navigating = false;
                OnSelected?.Invoke();
                selectAction.performed -= Select;
            }
        }
    }
}
