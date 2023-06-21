using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class EnemySpawning : MonoBehaviour
    {
        [SerializeField]
        private CombatEnemyUnit prefab;

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

            foreach (EnemyData data in combatInfo.Enemies)
            {
                CombatEnemyUnit enemy = SpawnEnemyOfType(data.Type);
                enemy.transform.position = enemyPositions[positionIndex].position;
                enemy.transform.rotation = enemyPositions[positionIndex].rotation;

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
                    spawned = Instantiate(prefab);
                    break;
            }

            return spawned;
        }
    }
}
