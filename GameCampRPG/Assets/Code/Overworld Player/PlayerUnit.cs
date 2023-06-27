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

        private Vector2 move;

        [SerializeField]
        private InteractionSensor interactionSensor;

        protected override void Awake()
        {
            if (GameInstance.Instance == null) return;

            base.Awake();

            inputs = GameInstance.Instance.GetPlayerInfo().PlayerInputs;
            moveAction = inputs.Overworld.Move;
            interact = inputs.Overworld.Interact;
        }

        private void OnEnable()
        {
            if (GameInstance.Instance == null) return;

            interact.performed += Interact;
        }

        private void OnDisable()
        {
            if (GameInstance.Instance == null) return;

            interact.performed -= Interact;
        }

        private void FixedUpdate()
        {
            move = moveAction.ReadValue<Vector2>();

            if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
            {
                move = new Vector2(move.x, 0);
            }
            else if (Mathf.Abs(move.x) <= Mathf.Abs(move.y))
            {
                move = new Vector2(0, move.y);
            }

            move.Normalize();

            mover.Move(move);
        }

        public void Interact(InputAction.CallbackContext callback)
        {
            interactionSensor.IntersectingObject.Interact();
        }
    }
}
