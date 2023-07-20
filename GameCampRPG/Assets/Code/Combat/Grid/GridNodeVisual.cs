using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameCampRPG
{
    public class GridNodeVisual : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text targetedCount;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Highlight(bool state)
        {
            animator.SetBool("Highlighted", state);
        }

        public void SetTargetedCount(int amount)
        {
            if (targetedCount != null)
            {
                targetedCount.text = amount.ToString();
                if (amount == 0)
                {
                    targetedCount.text = "";
                }
            }
        }
    }
}
