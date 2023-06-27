using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class CombatPlayerBuffManager : MonoBehaviour
    {
        private bool movementBuff = false, attackBuff = false, skillBuff = false;

        public bool MovementBuff
        {
            get { return movementBuff; }
        }
        public bool AttackBuff
        {
            get { return attackBuff; }
        }
        public bool SkillBuff
        {
            get { return skillBuff; }
        }

        public event Action<bool> MovementBuffActivated;
        public event Action<bool> AttackBuffActivated;
        public event Action<bool> SkillBuffActivated;

        public void ActivateMovementBuff(bool state)
        {
            movementBuff = state;
            MovementBuffActivated?.Invoke(state);
        }
        public void ActivateAttackBuff(bool state)
        {
            attackBuff = state;
            AttackBuffActivated?.Invoke(state);
        }
        public void ActivateSkillBuff(bool state)
        {
            skillBuff = state;
            SkillBuffActivated?.Invoke(state);
        }
    }
}
