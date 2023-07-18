using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class GridNodeVisual : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Highlight(bool state)
        {
            animator.SetBool("Highlighted", state);
        }
    }
}
