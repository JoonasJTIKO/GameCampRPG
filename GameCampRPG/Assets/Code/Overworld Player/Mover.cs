using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    [RequireComponent(typeof(Rigidbody))]
    public class Mover : MonoBehaviour
    {
        private Vector2 direction;

        private Rigidbody rb;

        public float Speed
        {
            get;
            private set;
        }

        public void Setup(float speed)
        {
            Speed = speed;

            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
            }
        }

        private void FixedUpdate()
        {
            Move(Time.fixedDeltaTime);
        }

        public void Move(Vector2 direction)
        {
            this.direction = direction;
        }

        private void Move(float deltaTime)
        {
            Vector2 movement = direction * Speed * deltaTime;
            Vector2 position = transform.position;
            position += movement;
            rb.MovePosition(position);
            direction = Vector2.zero;
        }
    }
}
