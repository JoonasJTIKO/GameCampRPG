using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class EnemySpawning : MonoBehaviour
    {
        [SerializeField]
        private CombatEnemyUnit snakePrefab;

        [SerializeField]
        private CombatEnemyUnit slimePrefab;

        [SerializeField]
        private CombatEnemyUnit treePrefab;

        [SerializeField]
        private CombatEnemyUnit bossPrefab;

        [SerializeField]
        private Transform[] enemyPositions;

        private bool skipThird = false;

        public List<CombatEnemyUnit> InitializeEnemies()
        {
            CombatInfo combatInfo = GameInstance.Instance.GetCombatInfo();
            if (combatInfo.Enemies.Count == 0) return null;

            List<CombatEnemyUnit> enemies = new List<CombatEnemyUnit>();
            int positionIndex = 0;
            if (combatInfo.Enemies.Count == 3 || combatInfo.Enemies.Count == 2) positionIndex++;
            else if (combatInfo.Enemies.Count == 1) positionIndex = 2;
            if (combatInfo.Enemies.Count % 2 == 0) skipThird = true;

            int unitIndex = 0;

            GameInstance.Instance.GetEnemyInfoCanvas().ActivatePanels(combatInfo.Enemies.Count);

            foreach (EnemyData data in combatInfo.Enemies)
            {
                CombatEnemyUnit enemy = SpawnEnemyOfType(data.Type);
                
                enemy.transform.position = enemyPositions[positionIndex].position;
                enemy.transform.rotation = enemyPositions[positionIndex].rotation;
                enemy.UnitIndex = unitIndex;
                SetEnemyDifficulty(enemy, data.Difficulty);
                unitIndex++;

                if (skipThird && positionIndex == 1) positionIndex = 3;
                else
                {
                    positionIndex++;
                }

                enemies.Add(enemy);
            }

            return enemies;
        }

        private CombatEnemyUnit SpawnEnemyOfType(EnemyType type)
        {
            CombatEnemyUnit spawned = null;

            switch (type)
            {
                case EnemyType.Snake:
                    spawned = Instantiate(snakePrefab);
                    break;
                case EnemyType.Slime:
                    spawned = Instantiate(slimePrefab);
                    break;
                case EnemyType.Tree:
                    spawned = Instantiate(treePrefab);
                    break;
                case EnemyType.Boss:
                    spawned = Instantiate(bossPrefab);
                    break;
            }

            return spawned;
        }

        private void SetEnemyDifficulty(CombatEnemyUnit enemy, EnemyDifficulty difficulty)
        {
            switch (difficulty)
            {
                case EnemyDifficulty.Easy:
                    enemy.ChangeStats(0);
                    break;
                case EnemyDifficulty.Normal:
                    enemy.ChangeStats(1);
                    break;
                case EnemyDifficulty.Hard:
                    enemy.ChangeStats(2);
                    break;
            }
        }
    }
}
