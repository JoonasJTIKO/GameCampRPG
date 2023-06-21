using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCampRPG
{
    public class EnemyGridTargeting : MonoBehaviour
    {
        public enum TargetingType
        {
            Line = 0,
        }

        public enum TargetingDirection
        {
            Up = 0,
            Down = 1,
            Left = 2,
            Right = 3,
        }

        private int startPosition = -1;

        private TargetingDirection targetingDirection = TargetingDirection.Up;

        private TargetingType targetingType = TargetingType.Line;

        private GridVisual grid;

        public event Action OnAttackFinished;

        private void Awake()
        {
            grid = FindObjectOfType<GridVisual>();
        }

        public void Target(int position, TargetingType type, TargetingDirection direction = TargetingDirection.Up)
        {
            startPosition = position;
            targetingDirection = direction;
            targetingType = type;

            switch (type)
            {
                case TargetingType.Line:
                    switch (direction)
                    {
                        case TargetingDirection.Up:
                            for (int i = 0; i < grid.Size; i++)
                            {
                                grid.TargetNode(position, i);
                            }
                            break;
                        case TargetingDirection.Down:
                            for (int i = grid.Size - 1; i >= 0; i--)
                            {
                                grid.TargetNode(position, i);
                            }
                            break;
                        case TargetingDirection.Left:
                            for (int i = grid.Size - 1; i >= 0; i--)
                            {
                                grid.TargetNode(i, position);
                            }
                            break;
                        case TargetingDirection.Right:
                            for (int i = 0; i < grid.Size; i++)
                            {
                                grid.TargetNode(i, position);
                            }
                            break;
                    }
                    break;
            }
        }

        public void UnTarget()
        {
            if (startPosition == -1) return;

            switch (targetingType)
            {
                case TargetingType.Line:
                    switch (targetingDirection)
                    {
                        case TargetingDirection.Up:
                            for (int i = 0; i < grid.Size; i++)
                            {
                                grid.TargetNode(startPosition, i, false);
                            }
                            break;
                        case TargetingDirection.Down:
                            for (int i = grid.Size - 1; i >= 0; i--)
                            {
                                grid.TargetNode(startPosition, i, false);
                            }
                            break;
                        case TargetingDirection.Left:
                            for (int i = grid.Size - 1; i >= 0; i--)
                            {
                                grid.TargetNode(i, startPosition, false);
                            }
                            break;
                        case TargetingDirection.Right:
                            for (int i = 0; i < grid.Size; i++)
                            {
                                grid.TargetNode(i, startPosition, false);
                            }
                            break;
                    }
                    break;
            }
        }

        public void AttackTargeted()
        {
            StartCoroutine(Attack());
        }

        private IEnumerator Attack()
        {
            switch (targetingType)
            {
                case TargetingType.Line:
                    switch (targetingDirection)
                    {
                        case TargetingDirection.Up:
                            for (int i = 0; i < grid.Size; i++)
                            {
                                AttackNode(startPosition, i);
                                grid.TargetNode(startPosition, i, false);
                                yield return new WaitForSeconds(0.2f);
                            }
                            break;
                        case TargetingDirection.Down:
                            for (int i = grid.Size - 1; i >= 0; i--)
                            {
                                AttackNode(startPosition, i);
                                grid.TargetNode(startPosition, i, false);
                                yield return new WaitForSeconds(0.2f);
                            }
                            break;
                        case TargetingDirection.Left:
                            for (int i = grid.Size - 1; i >= 0; i--)
                            {
                                AttackNode(i, startPosition);
                                grid.TargetNode(i, startPosition, false);
                                yield return new WaitForSeconds(0.2f);
                            }
                            break;
                        case TargetingDirection.Right:
                            for (int i = 0; i < grid.Size; i++)
                            {
                                AttackNode(i, startPosition);
                                grid.TargetNode(i, startPosition, false);
                                yield return new WaitForSeconds(0.2f);
                            }
                            break;
                    }
                    break;
            }

            OnAttackFinished?.Invoke();
        }

        private void AttackNode(int x, int y)
        {
            GameObject unit = grid.CheckForPlayer(x, y);
            if (unit != null)
            {
                unit.GetComponent<CombatPlayerUnit>().TakeDamage(1);
            }
        }
    }
}
