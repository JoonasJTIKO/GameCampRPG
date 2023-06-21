using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public abstract class CombatActionBase : MonoBehaviour
    {
        public bool Activated { get; protected set; }

        public static event Action<bool> OnActionExecuted;

        protected bool listening = false;

        protected bool extraAction = false;

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
            return true;
        }

        public virtual void Execute()
        {
            Activated = false;
        }

        protected void Executed()
        {
            OnActionExecuted?.Invoke(extraAction);
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
    }
}
