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

        private PowerUpSpawning powerUpSpawning;

        private bool actionExecuting = false;

        private bool playerTurn = true;

        private int playerCount, enemyCount;

        private Coroutine transitionRoutine;

        private int index = 0;

        private void Awake()
        {
            if (GameInstance.Instance == null) return;

            unitSelection = GetComponent<UnitSelection>();
            enemySpawning = GetComponent<EnemySpawning>();
            powerUpSpawning = GetComponent<PowerUpSpawning>();
            EnemyUnits = enemySpawning.InitializeEnemies();
        }

        private void Start()
        {
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
                if (unit.QueuedAction == null && unit.IsAlive) return;
            }
            if (playerTurn) EndPlayerTurn();
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

            foreach (IItem item in GameInstance.Instance.GetCombatInfo().Drops)
            {
                GameInstance.Instance.GetPlayerInfo().PlayerInventory.AddItems(item);
            }

            GameInstance.Instance.GetEnemyInfoCanvas().DeactivatePanels();
            GameInstance.Instance.GetPlayerCombatCanvas().Hide();
            audioListener.enabled = false;
            transitionRoutine = StartCoroutine(Transition());
        }

        private void StartPlayerTurn()
        {
            playerTurn = true;

            if (transitionRoutine != null) return;

            powerUpSpawning.SpawnPowerUp();

            foreach (CombatPlayerUnit unit in playerUnits)
            {
                unit.IsSelectable = true;
                unit.ProgressCooldowns();
            }
            unitSelection.SwitchTargetingMode(UnitSelection.TargetingMode.PlayerUnits);
        }

        private void StartEnemyTurn()
        {
            unitSelection.SwitchTargetingMode(UnitSelection.TargetingMode.None, defaultCamera: true);
            StartCoroutine(ExecuteEnemyActions());
        }

        public void EndPlayerTurn()
        {
            playerTurn = false;
            unitSelection.SwitchTargetingMode(UnitSelection.TargetingMode.None, defaultCamera: true);
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
