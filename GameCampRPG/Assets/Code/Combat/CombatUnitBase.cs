using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCampRPG
{
    public abstract class CombatUnitBase : MonoBehaviour
    {
        [SerializeField]
        protected int maxHealth = 3;

        [SerializeField]
        protected int attackStrength = 1;

        [SerializeField]
        protected int skillStrength = 1;

        protected List<CombatActionBase> combatActions;

        public CombatActionBase QueuedAction = null;

        public event Action OnDied;

        public int Health
        {
            get;
            protected set;
        }

        public int AttackStrength
        {
            get { return attackStrength; }
        }

        public int SkillStrength
        {
            get { return skillStrength; }
        }

        public bool IsAlive
        {
            get { return Health > 0; }
        }

        protected virtual void Awake()
        {
            Health = maxHealth;
            combatActions = GetComponents<CombatActionBase>().ToList();
        }

        public virtual void ChangeStats(int amount)
        {
            maxHealth += amount;
            Health = maxHealth;

            attackStrength += amount;
            if (attackStrength > 2) attackStrength = 2;

            skillStrength += amount;
        }

        public virtual bool ChangeHealth(int amount)
        {
            if ((amount < 0 && Health == 0) || (amount > 0 && Health == maxHealth))
            {
                return false;
            }

            if (amount < 0 && Health + amount < 0)
            {
                Health = 0;
                Die();
                return true;
            }
            else if (amount > 0 && Health + amount > maxHealth)
            {
                Health = maxHealth;
                return true;
            }
            else
            {
                Health += amount;
                if (!IsAlive) Die();
                return true;
            }
        }

        public virtual bool ExecuteQueuedAction()
        {
            if (QueuedAction == null || !IsAlive) return false;

            QueuedAction.Execute();
            return true;
        }

        public virtual bool SetQueuedAction(CombatActionBase action)
        {
            if (QueuedAction != null) QueuedAction.DequeueAction();

            QueuedAction = action;
            return false;
        }

        public void ProgressCooldowns()
        {
            foreach (CombatActionBase combatAction in combatActions)
            {
                combatAction.ProgressCooldown();
            }
        }

        private void Die()
        {
            OnDied?.Invoke();
        }
    }
}
