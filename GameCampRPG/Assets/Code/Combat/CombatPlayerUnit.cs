using GameCampRPG.UI;
//using Packages.Rider.Editor.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class CombatPlayerUnit : CombatUnitBase
    {
        [SerializeField]
        private int characterIndex = 0;

        public CombatActionBase ExtraAction = null;

        public bool DoExtraAction = false;

        public bool LockAction = false;

        public bool IsSelectable = true;

        private CombatPlayerBuffManager playerBuffManager;

        private PHEnemyHealthDisplay healthDisplay;

        private int defense = 1;

        protected override void Awake()
        {
            base.Awake();

            playerBuffManager = GetComponent<CombatPlayerBuffManager>();
            healthDisplay = GetComponentInChildren<PHEnemyHealthDisplay>();

            if (GameInstance.Instance == null) return;

            if (GameInstance.Instance.GetPlayerInfo().CharacterHealths[characterIndex] == 0)
            {
                GameInstance.Instance.GetPlayerInfo().SetCharacterHealth(characterIndex, Health);
            }
            else
            {
                Health = GameInstance.Instance.GetPlayerInfo().CharacterHealths[characterIndex];
            }

            if (GameInstance.Instance.GetPlayerInfo().AttackStrengths[characterIndex] == 0)
            {
                GameInstance.Instance.GetPlayerInfo().SetCharacterAttackStrength(characterIndex, attackStrength);
            }
            else
            {
                attackStrength = GameInstance.Instance.GetPlayerInfo().AttackStrengths[characterIndex];
            }

            if (GameInstance.Instance.GetPlayerInfo().SkillStrengths[characterIndex] == 0)
            {
                GameInstance.Instance.GetPlayerInfo().SetCharacterSkillStrength(characterIndex, skillStrength);
            }
            else
            {
                skillStrength = GameInstance.Instance.GetPlayerInfo().SkillStrengths[characterIndex];
            }

            if (GameInstance.Instance.GetPlayerInfo().Defenses[characterIndex] == 0)
            {
                GameInstance.Instance.GetPlayerInfo().SetCharacterDefense(characterIndex, defense);
            }
            else
            {
                defense = GameInstance.Instance.GetPlayerInfo().Defenses[characterIndex];
            }
        }

        private void Start()
        {
            if (GameInstance.Instance == null) return;

            GameInstance.Instance.GetPlayerCombatCanvas().SetHealthText(characterIndex, Health);
        }

        private void OnEnable()
        {
            playerBuffManager.AttackBuffActivated += ChangeAttackDamage;
            playerBuffManager.SkillBuffActivated += ChangeSkillStrength;
        }

        private void OnDisable()
        {
            playerBuffManager.AttackBuffActivated -= ChangeAttackDamage;
            playerBuffManager.SkillBuffActivated -= ChangeSkillStrength;
        }

        public override bool ExecuteQueuedAction()
        {
            if (QueuedAction == null || !IsAlive) return false;

            if (ExtraAction != null)
            {
                ExtraAction.Execute();
                return true;
            }

            QueuedAction.Execute();
            return true;
        }

        public void ActivateActionsMenu(bool state)
        {
            GameInstance.Instance.GetPlayerCombatCanvas().SelectUnit(characterIndex, state);

            if (state)
            {
                if (LockAction) return;

                foreach (CombatActionBase action in combatActions)
                {
                    action.BeginListening();
                }
            }
            else
            {
                foreach (CombatActionBase action in combatActions)
                {
                    action.StopListening();
                }
            }
        }

        public void TakeDamage(int amount)
        {
            amount = amount / defense;

            ChangeHealth(-amount);
            GameInstance.Instance.GetPlayerCombatCanvas().SetHealthText(characterIndex, Health);
        }

        public void UpdateSkillCooldown(int cooldown)
        {
            GameInstance.Instance.GetPlayerCombatCanvas().SetCooldownText(characterIndex, cooldown);
        }

        public override bool SetQueuedAction(CombatActionBase action)
        {
            if (ExtraAction != null && action == null)
            {
                ExtraAction.DequeueAction();
                ExtraAction = action;
                DoExtraAction = false;
                return false;
            }

            if (DoExtraAction && ExtraAction == null)
            {
                ExtraAction = action;
                return true;
            }
            base.SetQueuedAction(action);
            return false;
        }

        public override bool ChangeHealth(int amount)
        {
            bool returnValue = base.ChangeHealth(amount);
            healthDisplay.UpdateText(Health.ToString());
            return returnValue;
        }

        private void ChangeAttackDamage(bool input)
        {
            if (!input && attackStrength != GameInstance.Instance.GetPlayerInfo().AttackStrengths[characterIndex])
            {
                attackStrength--;
            }
            else if (input && attackStrength == GameInstance.Instance.GetPlayerInfo().AttackStrengths[characterIndex])
            {
                attackStrength++;
            }
        }

        private void ChangeSkillStrength(bool input)
        {
            if (!input && skillStrength != GameInstance.Instance.GetPlayerInfo().SkillStrengths[characterIndex])
            {
                skillStrength--;
            }
            else if (input && skillStrength == GameInstance.Instance.GetPlayerInfo().SkillStrengths[characterIndex])
            {
                skillStrength++;
            }
        }
    }
}
