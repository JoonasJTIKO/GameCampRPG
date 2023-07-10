using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class PositionRenderSort : MonoBehaviour
    {
        [SerializeField]
        private int sortingOrderBase = 5000;

        [SerializeField]
        private int offset;

        [SerializeField]
        private bool runOnce = false;

        private Renderer myRenderer;

        private void Awake()
        {
            myRenderer = GetComponentInChildren<Renderer>();
        }

        private void LateUpdate()
        {
            myRenderer.sortingOrder = (int)(sortingOrderBase - transform.position.y - offset);
            if (runOnce)
            {
                this.enabled = false;
            }
        }
    }
}
