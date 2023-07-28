using GameCampRPG.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameCampRPG
{
    public class PlayerUnit : UnitBase
    {
        private PlayerInputs inputs;

        private InputAction moveAction;

        private InputAction interact;

        private InputAction openPauseMenu;

        private Vector2 move;

        private Animator animator;

        [SerializeField]
        private InteractionSensor interactionSensor;

        protected override void Awake()
        {
            if (GameInstance.Instance == null) return;

            base.Awake();

            inputs = GameInstance.Instance.GetPlayerInfo().PlayerInputs;
            moveAction = inputs.Overworld.Move;
            openPauseMenu = inputs.Overworld.Pause;
            interact = inputs.Overworld.Interact;
            animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            if (GameInstance.Instance == null) return;

            interact.performed += Interact;
            openPauseMenu.performed += OpenPauseMenu;
        }

        private void OnDisable()
        {
            if (GameInstance.Instance == null) return;

            interact.performed -= Interact;
            openPauseMenu.performed -= OpenPauseMenu;
        }

        private void FixedUpdate()
        {
            move = moveAction.ReadValue<Vector2>();

            if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
            {
                move = new Vector2(move.x, 0);
                animator.SetBool("Idle", false);
                animator.SetFloat("X", move.x);
                animator.SetFloat("Y", move.y);
            }
            else if (Mathf.Abs(move.x) <= Mathf.Abs(move.y))
            {
                move = new Vector2(0, move.y);
                animator.SetBool("Idle", false);
                animator.SetFloat("X", move.x);
                animator.SetFloat("Y", move.y);
            }

            if (move == Vector2.zero) animator.SetBool("Idle", true);

            move.Normalize();

            mover.Move(move);
        }

        public void Interact(InputAction.CallbackContext callback)
        {
            if (interactionSensor.IntersectingObject == null) return;

            interactionSensor.IntersectingObject.Interact();
        }

        public void OpenPauseMenu(InputAction.CallbackContext callback)
        {
            if (Time.timeScale != 0f)
            {
                GameInstance.Instance.GetPauseMenuCanvas().Show();
                Time.timeScale = 0f;
            }
            else
            {
                PauseMenuCanvas canvas = GameInstance.Instance.GetPauseMenuCanvas();
                canvas.GetComponentInChildren<PauseMenuNavigation>().RemoveInputs();
                ItemInfo itemInfo = canvas.GetComponentInChildren<ItemInfo>();
                if (itemInfo != null)
                {
                    itemInfo.ClearInfo();
                }
                PlayerInventoryUI playerInventory = canvas.GetComponentInChildren<PlayerInventoryUI>();
                if (playerInventory != null)
                {
                    playerInventory.DisableInputs();
                }
                InventoryContextMenu iContextMenu = canvas.GetComponentInChildren<InventoryContextMenu>();
                if (iContextMenu != null)
                {
                    iContextMenu.RemoveInputs();
                    iContextMenu.Hide();
                }
                canvas.Hide();
                Time.timeScale = 1f;
            }
        }
    }
}
