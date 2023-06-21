using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCampRPG
{
    public class SelectCombatAction : MonoBehaviour
    {
        private List<CombatActionBase> combatActions;

        private CombatActionBase selectedAction = null;

        public bool ActionSelected { get; private set; }

        private void Awake()
        {
            combatActions = GetComponents<CombatActionBase>().ToList();
        }

        public bool SelectAction(int actionIndex)
        {
            if (actionIndex >= combatActions.Count)
            {
                Debug.LogWarning("Combat action index is higher than the max!");
                return false;
            }

            if (combatActions[actionIndex].QueueAction())
            {
                selectedAction = combatActions[actionIndex];
                ActionSelected = true;
                return true;
            }
            return false;
        }

        public bool DeselectAction(int actionIndex)
        {
            if (actionIndex >= combatActions.Count)
            {
                Debug.LogWarning("Combat action index is higher than the max!");
                return false;
            }

            if (combatActions[actionIndex].DequeueAction())
            {
                selectedAction = null;
                ActionSelected = false;
                return true;
            }
            return false;
        }

        public void ExecuteSelectedAction()
        {
            if (selectedAction != null)
            {
                selectedAction.Execute();
            }
        }
    }
}
