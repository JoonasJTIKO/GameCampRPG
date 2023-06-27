using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameCampRPG
{
    public abstract class CombatActionBase : MonoBehaviour
    {
        [SerializeField]
        protected int cooldown = 0;

        public bool Activated { get; protected set; }

        public int CurrentCooldown { get { return counter; } }

        public static event Action<bool> OnActionExecuted;

        protected bool listening = false;

        protected bool extraAction = false;

        protected bool onCooldown = false;

        private int counter = 0;

        public virtual bool QueueAction()
        {
            if (Activated) return false;

            Activated = true;
            return true;
        }

        public virtual bool DequeueAction()
        {
            if (!Activated) return false;

            Activated = false;
            extraAction = false;
            return true;
        }

        public virtual void Execute()
        {
            Activated = false;
        }

        protected void Executed()
        {
            OnActionExecuted?.Invoke(extraAction);
            ActivateCooldown();
            extraAction = false;
        }

        public virtual void BeginListening()
        {
            if (listening) return;

            listening = true;
        }

        public virtual void StopListening()
        {
            if (!listening) return;

            listening = false;
        }

        protected void ActivateCooldown()
        {
            counter = cooldown + 1;
            onCooldown = true;
        }

        public void ProgressCooldown()
        {
            if (onCooldown)
            {
                counter--;
                if (counter == 0) onCooldown = false;
            }
        }
    }
}
