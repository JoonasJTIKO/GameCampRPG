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

        protected override void Awake()
        {
            base.Awake();

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
                GameInstance.Instance.GetPlayerInfo().SetCharacterSkillStrength(characterIndex, skillStrenght);
            }
            else
            {
                skillStrenght = GameInstance.Instance.GetPlayerInfo().SkillStrengths[characterIndex];
            }
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
                ui.SetUnitText("Unit " + (characterIndex + 1));
                foreach (CombatActionBase action in combatActions)
                {
                    action.BeginListening();
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
    }
}
