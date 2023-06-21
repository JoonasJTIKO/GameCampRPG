using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG.UI
{
    public class SceneFadeCanvas : MenuCanvas
    {
        private Animator animator;

        private bool activated = false;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        public void FadeIn()
        {
            animator.SetTrigger("FadeIn");
            activated = true;
        }

        public void FadeOut()
        {
            if (!activated) return;

            animator.SetTrigger("FadeOut");
            activated = false;
        }
    }
}
