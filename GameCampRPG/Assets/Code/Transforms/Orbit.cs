using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class Orbit : MonoBehaviour
    {
        [SerializeField]
        private Transform orbitTarget;

        [SerializeField]
        private float orbitDistance;

        [SerializeField]
        private float orbitSpeed;

        [SerializeField]
        private float offset = 0f;

        private float angle;

        private void Start()
        {
            angle += offset;
        }

        private void Update()
        {
            Vector3 orbitPosition = orbitTarget.position +
                new Vector3(
                    Mathf.Cos(angle) * orbitDistance,
                    0f,
                    Mathf.Sin(angle) * orbitDistance);

            transform.position = orbitPosition;
            angle += orbitSpeed * Time.deltaTime;
        }
    }
}
