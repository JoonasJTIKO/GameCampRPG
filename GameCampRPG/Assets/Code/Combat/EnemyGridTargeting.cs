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
            Square = 1,
            Cross = 2
        }

        public enum TargetingDirection
        {
            Up = 0,
            Down = 1,
            Left = 2,
            Right = 3,
        }

        private int startPosition = -1;

        private int startX, startY;

        private TargetingDirection targetingDirection = TargetingDirection.Up;

        private TargetingType targetingType = TargetingType.Line;

        private GridVisual grid;

        public event Action OnAttackFinished;

        private void Awake()
        {
            grid = FindObjectOfType<GridVisual>();
        }

        public void Target(int position, TargetingType type, TargetingDirection direction = TargetingDirection.Up, int startX = 0, int startY = 0)
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

                case TargetingType.Square:
                    this.startX = startX;
                    this.startY = startY;

                    grid.TargetNode(startX, startY);
                    grid.TargetNode(startX + 1, startY);
                    grid.TargetNode(startX, startY + 1);
                    grid.TargetNode(startX + 1, startY + 1);
                    break;

                case TargetingType.Cross:
                    this.startX = startX;
                    this.startY = startY;

                    grid.TargetNode(startX, startY);
                    grid.TargetNode(startX + 1, startY);
                    grid.TargetNode(startX, startY + 1);
                    grid.TargetNode(startX - 1, startY);
                    grid.TargetNode(startX, startY - 1);
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

                case TargetingType.Square:
                    grid.TargetNode(startX, startY, false);
                    grid.TargetNode(startX + 1, startY, false);
                    grid.TargetNode(startX, startY + 1, false);
                    grid.TargetNode(startX + 1, startY + 1, false);
                    break;

                case TargetingType.Cross:
                    grid.TargetNode(startX, startY, false);
                    grid.TargetNode(startX + 1, startY, false);
                    grid.TargetNode(startX, startY + 1, false);
                    grid.TargetNode(startX - 1, startY, false);
                    grid.TargetNode(startX, startY - 1, false);
                    break;
            }
            startPosition = -1;
        }

        public void HighlightTargeted(bool state)
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
                                grid.HighlightNode(startPosition, i, state);
                            }
                            break;
                        case TargetingDirection.Down:
                            for (int i = grid.Size - 1; i >= 0; i--)
                            {
                                grid.HighlightNode(startPosition, i, state);
                            }
                            break;
                        case TargetingDirection.Left:
                            for (int i = grid.Size - 1; i >= 0; i--)
                            {
                                grid.HighlightNode(i, startPosition, state);
                            }
                            break;
                        case TargetingDirection.Right:
                            for (int i = 0; i < grid.Size; i++)
                            {
                                grid.HighlightNode(i, startPosition, state);
                            }
                            break;
                    }
                    break;

                case TargetingType.Square:
                    grid.HighlightNode(startX, startY, state);
                    grid.HighlightNode(startX + 1, startY, state);
                    grid.HighlightNode(startX, startY + 1, state);
                    grid.HighlightNode(startX + 1, startY + 1, state);
                    break;

                case TargetingType.Cross:
                    grid.HighlightNode(startX, startY, state);
                    grid.HighlightNode(startX + 1, startY, state);
                    grid.HighlightNode(startX, startY + 1, state);
                    grid.HighlightNode(startX - 1, startY, state);
                    grid.HighlightNode(startX, startY - 1, state);
                    break;
            }
        }

        public void AttackTargeted(int damage)
        {
            StartCoroutine(Attack(damage));
        }

        private IEnumerator Attack(int damage)
        {
            switch (targetingType)
            {
                case TargetingType.Line:
                    switch (targetingDirection)
                    {
                        case TargetingDirection.Up:
                            for (int i = 0; i < grid.Size; i++)
                            {
                                AttackNode(startPosition, i, damage);
                                grid.TargetNode(startPosition, i, false);
                                yield return new WaitForSeconds(0.2f);
                            }
                            break;
                        case TargetingDirection.Down:
                            for (int i = grid.Size - 1; i >= 0; i--)
                            {
                                AttackNode(startPosition, i, damage);
                                grid.TargetNode(startPosition, i, false);
                                yield return new WaitForSeconds(0.2f);
                            }
                            break;
                        case TargetingDirection.Left:
                            for (int i = grid.Size - 1; i >= 0; i--)
                            {
                                AttackNode(i, startPosition, damage);
                                grid.TargetNode(i, startPosition, false);
                                yield return new WaitForSeconds(0.2f);
                            }
                            break;
                        case TargetingDirection.Right:
                            for (int i = 0; i < grid.Size; i++)
                            {
                                AttackNode(i, startPosition, damage);
                                grid.TargetNode(i, startPosition, false);
                                yield return new WaitForSeconds(0.2f);
                            }
                            break;
                    }
                    break;

                case TargetingType.Square:
                    AttackNode(startX, startY, damage);
                    grid.TargetNode(startX, startY, false);
                    AttackNode(startX + 1, startY, damage);
                    grid.TargetNode(startX + 1, startY, false);
                    AttackNode(startX, startY + 1, damage);
                    grid.TargetNode(startX, startY + 1, false);
                    AttackNode(startX + 1, startY + 1, damage);
                    grid.TargetNode(startX + 1, startY + 1, false);
                    yield return new WaitForSeconds(0.2f);
                    break;

                case TargetingType.Cross:
                    AttackNode(startX, startY, damage);
                    grid.TargetNode(startX, startY, false);
                    yield return new WaitForSeconds(0.2f);

                    AttackNode(startX + 1, startY, damage);
                    grid.TargetNode(startX + 1, startY, false);
                    AttackNode(startX, startY + 1, damage);
                    grid.TargetNode(startX, startY + 1, false);
                    AttackNode(startX - 1, startY, damage);
                    grid.TargetNode(startX - 1, startY, false);
                    AttackNode(startX, startY - 1, damage);
                    grid.TargetNode(startX, startY - 1, false);
                    yield return new WaitForSeconds(0.2f);
                    break;
            }
            startPosition = -1;
            OnAttackFinished?.Invoke();
        }

        private void AttackNode(int x, int y, int damage)
        {
            GameObject unit = grid.CheckForPlayer(x, y);
            if (unit != null)
            {
                unit.GetComponent<CombatPlayerUnit>().TakeDamage(damage);
            }
        }
    }
}
