using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class GridNodeVisual : MonoBehaviour
    {
        private bool highlighted = false, growing = true;

        private float startScale;

        [SerializeField]
        private float animationSpeed = 1f;

        private void Awake()
        {
            startScale = transform.localScale.x;
        }

        public void Highlight(bool state)
        {
            highlighted = state;

            if (!highlighted) transform.localScale = new(startScale, startScale, startScale);

        }

        private void Update()
        {
            if (!highlighted) return;

            float deltaTime = Time.deltaTime;

            if (growing)
            {
                transform.localScale = 
                    new Vector3(transform.localScale.x + (deltaTime * animationSpeed), 
                    transform.localScale.y + (deltaTime * animationSpeed), 
                    transform.localScale.z + (deltaTime * animationSpeed));
                if (transform.localScale.x >= 1f)
                {
                    growing = false;
                    transform.localScale = new(1, 1, 1);
                }
            }
            else if (!growing)
            {
                transform.localScale =
                    new Vector3(transform.localScale.x - (deltaTime * animationSpeed),
                    transform.localScale.y - (deltaTime * animationSpeed),
                    transform.localScale.z - (deltaTime * animationSpeed));
                if (transform.localScale.x <= startScale)
                {
                    growing = true;
                    transform.localScale = new(startScale, startScale, startScale);
                }
            }
        }
    }
}
