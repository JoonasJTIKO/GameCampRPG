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

        private void Start()
        {
            thisTransform = transform;
        }

        private void Update()
        {
            Vector3 rotation;

            rotation = thisTransform.rotation.eulerAngles;
            rotation.y += (rotationDegrees * Time.deltaTime);
            thisTransform.eulerAngles = rotation;
        }
    }
}
