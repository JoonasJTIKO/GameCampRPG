using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class PowerUpSpawning : MonoBehaviour
    {
        private GridVisual grid;

        [SerializeField]
        private PowerUp attackPowerUp;

        [SerializeField]
        private PowerUp skillPowerUp;

        [SerializeField]
        private PowerUp movementPowerUp;

        public PowerUp Spawned = null;

        private PowerUpType previousPowerUpType = PowerUpType.None;

        private void Awake()
        {
            grid = FindObjectOfType<GridVisual>();
        }

        public void SpawnPowerUp()
        {
            if (Spawned != null) return;

            int x, y;

            do
            {
                x = Random.Range(0, grid.Size - 2);
                y = Random.Range(0, grid.Size);
            }
            while (grid.CheckForPlayer(x, y) != null);

            PowerUpType powerUpType;

            do
            {
                powerUpType = (PowerUpType)Random.Range(1, 4);
            }
            while (powerUpType == previousPowerUpType);

            switch (powerUpType)
            {
                case PowerUpType.Attack:
                    Spawned = attackPowerUp;
                    attackPowerUp.transform.position = grid.GetNodePosition(x, y);
                    attackPowerUp.transform.position = new(attackPowerUp.transform.position.x, attackPowerUp.transform.position.y + 0.5f, attackPowerUp.transform.position.z);
                    attackPowerUp.Activate();
                    break;
                case PowerUpType.Skill:
                    Spawned = skillPowerUp;
                    skillPowerUp.transform.position = grid.GetNodePosition(x, y);
                    skillPowerUp.transform.position = new(skillPowerUp.transform.position.x, skillPowerUp.transform.position.y + 0.5f, skillPowerUp.transform.position.z);
                    skillPowerUp.Activate();
                    break;
                case PowerUpType.Movement:
                    Spawned = movementPowerUp;
                    movementPowerUp.transform.position = grid.GetNodePosition(x, y);
                    movementPowerUp.transform.position = new(movementPowerUp.transform.position.x, movementPowerUp.transform.position.y + 0.5f, movementPowerUp.transform.position.z);
                    movementPowerUp.Activate();
                    break;
            }
        }
    }
}
