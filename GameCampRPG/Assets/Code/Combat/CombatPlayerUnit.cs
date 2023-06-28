using GameCampRPG.UI;
using Packages.Rider.Editor.UnitTesting;
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
        }

        private void Start()
        {
            healthDisplay.UpdateText(Health.ToString());
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
            if (state)
            {
                gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

                if (LockAction) return;

                PlayerCombatCanvas ui = GameInstance.Instance.GetPlayerCombatCanvas();
                ui.Show();

                switch (characterIndex)
                {
                    case 0:
                        ui.SetUnitText("Rogue");
                        break;
                    case 1:
                        ui.SetUnitText("Knight");
                        break;
                    case 2:
                        ui.SetUnitText("Mage");
                        break;
                }

                ui.SetCooldownText(0);
                foreach (CombatActionBase action in combatActions)
                {
                    action.BeginListening();

                    if (action.CurrentCooldown != 0)
                    {
                        ui.SetCooldownText(action.CurrentCooldown);
                    }
                }
            }
            else
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                PlayerCombatCanvas ui = GameInstance.Instance.GetPlayerCombatCanvas();
                ui.Hide();
                foreach (CombatActionBase action in combatActions)
                {
                    action.StopListening();
                }
            }
        }

        public void TakeDamage(int amount)
        {
            ChangeHealth(-amount);
            healthDisplay.UpdateText(Health.ToString());
            Debug.Log("unit " + characterIndex + " Took " + amount + " damage");
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
