using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class Rotate : MonoBehaviour
    {
        private Transform thisTransform;

        [SerializeField]
        private float rotationDegrees = 1f;

        [SerializeField]
        private bool counterClockwise = false;

        private void Start()
        {
            thisTransform = transform;
        }

        private void Update()
        {
            Vector3 rotation;

            rotation = thisTransform.rotation.eulerAngles;

            if (counterClockwise)
            {
                rotation.y -= (rotationDegrees * Time.deltaTime);
            }
            else
            {
                rotation.y += (rotationDegrees * Time.deltaTime);
            }
            thisTransform.eulerAngles = rotation;
        }
    }
}
