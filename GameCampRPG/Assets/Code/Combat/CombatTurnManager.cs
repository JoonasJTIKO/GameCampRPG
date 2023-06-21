using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameCampRPG
{
    public class CombatTurnManager : MonoBehaviour
    {
        [SerializeField]
        private List<CombatPlayerUnit> playerUnits;

        [SerializeField]
        private AudioListener audioListener;

        public List<CombatEnemyUnit> EnemyUnits
        {
            get;
            private set;
        }

        private UnitSelection unitSelection;

        private EnemySpawning enemySpawning;

        private bool actionExecuting = false;

        private int playerCount, enemyCount;

        private Coroutine transitionRoutine;

        private int index = 0;

        private void Awake()
        {
            if (GameInstance.Instance == null) return;

            unitSelection = GetComponent<UnitSelection>();
            enemySpawning = GetComponent<EnemySpawning>();
            EnemyUnits = enemySpawning.InitializeEnemies();

            StartPlayerTurn();
        }

        private void OnEnable()
        {
            if (GameInstance.Instance == null) return;

            CombatActionBase.OnActionExecuted += ActionFinished;

            foreach (CombatPlayerUnit playerUnit in playerUnits)
            {
                playerUnit.OnDied += PlayerUnitDied;
                playerCount++;
            }

            foreach (CombatEnemyUnit enemyUnit in EnemyUnits)
            {
                enemyUnit.OnDied += EnemyUnitDied;
                enemyCount++;
            }
        }

        private void OnDisable()
        {
            if (GameInstance.Instance == null) return;

            CombatActionBase.OnActionExecuted -= ActionFinished;

            foreach (CombatPlayerUnit playerUnit in playerUnits)
            {
                playerUnit.OnDied -= PlayerUnitDied;
            }

            foreach (CombatEnemyUnit enemyUnit in EnemyUnits)
            {
                enemyUnit.OnDied -= EnemyUnitDied;
            }
        }

        private void Update()
        {
            foreach (CombatPlayerUnit unit in playerUnits)
            {
                if (unit.QueuedAction == null) return;
            }
            EndPlayerTurn();
        }

        private void PlayerUnitDied()
        {
            playerCount--;
            if (playerCount == 0) EndCombat();
        }

        private void EnemyUnitDied()
        {
            enemyCount--;
            if (enemyCount == 0) EndCombat();
        }

        private void EndCombat()
        {
            GameInstance.Instance.GetPlayerInfo().PlayerInputs.Combat.Disable();
            GameInstance.Instance.GetPlayerCombatCanvas().Hide();
            audioListener.enabled = false;
            transitionRoutine = StartCoroutine(Transition());
        }

        private void StartPlayerTurn()
        {
            if (transitionRoutine != null) return;

            foreach (CombatPlayerUnit unit in playerUnits)
            {
                unit.IsSelectable = true;
            }
            unitSelection.SwitchTargetingMode(UnitSelection.TargetingMode.PlayerUnits);
        }

        private void StartEnemyTurn()
        {
            StartCoroutine(ExecuteEnemyActions());
        }

        public void EndPlayerTurn()
        {
            unitSelection.SwitchTargetingMode(UnitSelection.TargetingMode.None);
            StartCoroutine(ExecuteQueuedActions());
        }

        private void ActionStarted()
        {
            actionExecuting = true;
        }

        public void ActionFinished(bool extraAction)
        {
            if (extraAction) index--;

            actionExecuting = false;
        }

        private IEnumerator ExecuteQueuedActions()
        {
            index = 0;

            while (index < playerUnits.Count || actionExecuting)
            {
                if (!actionExecuting)
                {
                    ActionStarted();
                    if (!playerUnits[index].ExecuteQueuedAction())
                    {
                        ActionFinished(false);
                    }
                    index++;
                }
                yield return null;
            }

            StartEnemyTurn();
        }

        private IEnumerator ExecuteEnemyActions()
        {
            index = 0;

            while (index < EnemyUnits.Count)
            {
                if (!actionExecuting)
                {
                    ActionStarted();
                    if (!EnemyUnits[index].ExecuteQueuedAction())
                    {
                        ActionFinished(false);
                    }
                    index++;
                }
                yield return null;
            }

            StartCoroutine(QueueEnemyActions());
        }

        private IEnumerator QueueEnemyActions()
        {
            while (actionExecuting)
            {
                yield return null;
            }

            foreach (CombatEnemyUnit unit in EnemyUnits)
            {
                if (unit.IsAlive) unit.SelectAction();
            }
            StartPlayerTurn();
            yield return null;
        }

        private IEnumerator Transition()
        {
            GameInstance.Instance.GetSceneFadeCanvas().FadeIn();
            yield return new WaitForSeconds(1.1f);
            GameInstance.Instance.GetGameStateManager().Go(StateType.Overworld, loadScene: false);
        }
    }
}
