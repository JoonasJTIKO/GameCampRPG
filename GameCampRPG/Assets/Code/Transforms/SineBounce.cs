using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class SineBounce : MonoBehaviour
    {
        [SerializeField]
        private float bounceSpeed = 1f;

        [SerializeField]
        private float bounceDistance = 1f;

        [SerializeField]
        private bool absolute = true;

        public Vector3 origin;
        private float angle;

        private void Update()
        {
            angle += bounceSpeed * Time.deltaTime;
            float sine = Mathf.Sin(angle) * bounceDistance;

            if (absolute) sine = Mathf.Abs(sine);

            transform.position = origin + new Vector3(0f, sine, 0f);
        }
    }
}
