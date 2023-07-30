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
            Cross = 2,
            BigLine = 3,
            BigSquare = 4,
            BigCross = 5,
            FullGrid = 6,
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

        public void Target(int position, int damage, TargetingType type, TargetingDirection direction = TargetingDirection.Up, int startX = 0, int startY = 0)
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
                                grid.TargetNode(position, i, damage);
                            }
                            break;
                        case TargetingDirection.Down:
                            for (int i = grid.Size - 1; i >= 0; i--)
                            {
                                grid.TargetNode(position, i, damage);
                            }
                            break;
                        case TargetingDirection.Left:
                            for (int i = grid.Size - 1; i >= 0; i--)
                            {
                                grid.TargetNode(i, position, damage);
                            }
                            break;
                        case TargetingDirection.Right:
                            for (int i = 0; i < grid.Size; i++)
                            {
                                grid.TargetNode(i, position, damage);
                            }
                            break;
                    }
                    break;

                case TargetingType.Square:
                    this.startX = startX;
                    this.startY = startY;

                    grid.TargetNode(startX, startY, damage);
                    grid.TargetNode(startX + 1, startY, damage);
                    grid.TargetNode(startX, startY + 1, damage);
                    grid.TargetNode(startX + 1, startY + 1, damage);
                    break;

                case TargetingType.Cross:
                    this.startX = startX;
                    this.startY = startY;

                    grid.TargetNode(startX, startY, damage);
                    grid.TargetNode(startX + 1, startY, damage);
                    grid.TargetNode(startX, startY + 1, damage);
                    grid.TargetNode(startX - 1, startY, damage);
                    grid.TargetNode(startX, startY - 1, damage);
                    break;

                case TargetingType.BigLine:
                    switch (direction)
                    {
                        case TargetingDirection.Up:
                            for (int i = 0; i < grid.Size; i++)
                            {
                                grid.TargetNode(position, i, damage);
                                grid.TargetNode(position - 1, i, damage);
                                grid.TargetNode(position + 1, i, damage);
                            }
                            break;
                        case TargetingDirection.Down:
                            for (int i = grid.Size - 1; i >= 0; i--)
                            {
                                grid.TargetNode(position, i, damage);
                                grid.TargetNode(position - 1, i, damage);
                                grid.TargetNode(position + 1, i, damage);
                            }
                            break;
                        case TargetingDirection.Left:
                            for (int i = grid.Size - 1; i >= 0; i--)
                            {
                                grid.TargetNode(i, position, damage);
                                grid.TargetNode(i, position - 1, damage);
                                grid.TargetNode(i, position + 1, damage);
                            }
                            break;
                        case TargetingDirection.Right:
                            for (int i = 0; i < grid.Size; i++)
                            {
                                grid.TargetNode(i, position, damage);
                                grid.TargetNode(i, position - 1, damage);
                                grid.TargetNode(i, position + 1, damage);
                            }
                            break;
                    }
                    break;

                case TargetingType.BigSquare:
                    this.startX = startX;
                    this.startY = startY;

                    grid.TargetNode(startX, startY, damage);
                    grid.TargetNode(startX + 1, startY, damage);
                    grid.TargetNode(startX, startY + 1, damage);
                    grid.TargetNode(startX + 1, startY + 1, damage);
                    grid.TargetNode(startX + 1, startY - 1, damage);
                    grid.TargetNode(startX - 1, startY - 1, damage);
                    grid.TargetNode(startX, startY - 1, damage);
                    grid.TargetNode(startX - 1, startY, damage);
                    grid.TargetNode(startX - 1, startY + 1, damage);
                    break;

                case TargetingType.BigCross:
                    this.startX = startX;
                    this.startY = startY;

                    grid.TargetNode(startX, startY, damage);
                    grid.TargetNode(startX + 1, startY, damage);
                    grid.TargetNode(startX + 2, startY, damage);
                    grid.TargetNode(startX, startY + 1, damage);
                    grid.TargetNode(startX, startY + 2, damage);
                    grid.TargetNode(startX - 1, startY, damage);
                    grid.TargetNode(startX - 2, startY, damage);
                    grid.TargetNode(startX, startY - 1, damage);
                    grid.TargetNode(startX, startY - 2, damage);
                    grid.TargetNode(startX + 1, startY + 1, damage);
                    grid.TargetNode(startX + 2, startY + 2, damage);
                    grid.TargetNode(startX + 1, startY - 1, damage);
                    grid.TargetNode(startX + 2, startY - 2, damage);
                    grid.TargetNode(startX - 1, startY + 1, damage);
                    grid.TargetNode(startX - 2, startY + 2, damage);
                    grid.TargetNode(startX - 1, startY - 1, damage);
                    grid.TargetNode(startX - 2, startY - 2, damage);
                    break;

                case TargetingType.FullGrid:
                    for (int i = 0; i < grid.Size; i++)
                    {
                        for (int j = 0; j < grid.Size; j++)
                        {
                            grid.TargetNode(i, j, damage);
                        }
                    }
                    break;
            }
        }

        public void UnTarget(int damage)
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
                                grid.TargetNode(startPosition, i, -damage, false);
                            }
                            break;
                        case TargetingDirection.Down:
                            for (int i = grid.Size - 1; i >= 0; i--)
                            {
                                grid.TargetNode(startPosition, i, -damage, false);
                            }
                            break;
                        case TargetingDirection.Left:
                            for (int i = grid.Size - 1; i >= 0; i--)
                            {
                                grid.TargetNode(i, startPosition, -damage, false);
                            }
                            break;
                        case TargetingDirection.Right:
                            for (int i = 0; i < grid.Size; i++)
                            {
                                grid.TargetNode(i, startPosition, -damage, false);
                            }
                            break;
                    }
                    break;

                case TargetingType.Square:
                    grid.TargetNode(startX, startY, -damage, false);
                    grid.TargetNode(startX + 1, startY, -damage, false);
                    grid.TargetNode(startX, startY + 1, -damage, false);
                    grid.TargetNode(startX + 1, startY + 1, -damage, false);
                    break;

                case TargetingType.Cross:
                    grid.TargetNode(startX, startY, -damage, false);
                    grid.TargetNode(startX + 1, startY, -damage, false);
                    grid.TargetNode(startX, startY + 1, -damage, false);
                    grid.TargetNode(startX - 1, startY, -damage, false);
                    grid.TargetNode(startX, startY - 1, -damage, false);
                    break;

                case TargetingType.BigLine:
                    switch (targetingDirection)
                    {
                        case TargetingDirection.Up:
                            for (int i = 0; i < grid.Size; i++)
                            {
                                grid.TargetNode(startPosition, i, -damage, false);
                                grid.TargetNode(startPosition - 1, i, -damage, false);
                                grid.TargetNode(startPosition + 1, i, -damage, false);
                            }
                            break;
                        case TargetingDirection.Down:
                            for (int i = grid.Size - 1; i >= 0; i--)
                            {
                                grid.TargetNode(startPosition, i, -damage, false);
                                grid.TargetNode(startPosition - 1, i, -damage, false);
                                grid.TargetNode(startPosition + 1, i, -damage, false);
                            }
                            break;
                        case TargetingDirection.Left:
                            for (int i = grid.Size - 1; i >= 0; i--)
                            {
                                grid.TargetNode(i, startPosition, -damage, false);
                                grid.TargetNode(i, startPosition - 1, -damage, false);
                                grid.TargetNode(i, startPosition + 1, -damage, false);
                            }
                            break;
                        case TargetingDirection.Right:
                            for (int i = 0; i < grid.Size; i++)
                            {
                                grid.TargetNode(i, startPosition, -damage, false);
                                grid.TargetNode(i, startPosition - 1, -damage, false);
                                grid.TargetNode(i, startPosition + 1, -damage, false);
                            }
                            break;
                    }
                    break;

                case TargetingType.BigSquare:
                    grid.TargetNode(startX, startY, -damage, false);
                    grid.TargetNode(startX + 1, startY, -damage, false);
                    grid.TargetNode(startX, startY + 1, -damage, false);
                    grid.TargetNode(startX + 1, startY + 1, -damage, false);
                    grid.TargetNode(startX + 1, startY - 1, -damage, false);
                    grid.TargetNode(startX - 1, startY - 1, -damage, false);
                    grid.TargetNode(startX, startY - 1, -damage, false);
                    grid.TargetNode(startX - 1, startY, -damage, false);
                    grid.TargetNode(startX - 1, startY + 1, -damage, false);
                    break;

                case TargetingType.BigCross:
                    grid.TargetNode(startX, startY, -damage, false);
                    grid.TargetNode(startX + 1, startY, -damage, false);
                    grid.TargetNode(startX + 2, startY, -damage, false);
                    grid.TargetNode(startX, startY + 1, -damage, false);
                    grid.TargetNode(startX, startY + 2, -damage, false);
                    grid.TargetNode(startX - 1, startY, -damage, false);
                    grid.TargetNode(startX - 2, startY, -damage, false);
                    grid.TargetNode(startX, startY - 1, -damage, false);
                    grid.TargetNode(startX, startY - 2, -damage, false);
                    grid.TargetNode(startX + 1, startY + 1, -damage, false);
                    grid.TargetNode(startX + 2, startY + 2, -damage, false);
                    grid.TargetNode(startX + 1, startY - 1, -damage, false);
                    grid.TargetNode(startX + 2, startY - 2, -damage, false);
                    grid.TargetNode(startX - 1, startY + 1, -damage, false);
                    grid.TargetNode(startX - 2, startY + 2, -damage, false);
                    grid.TargetNode(startX - 1, startY - 1, -damage, false);
                    grid.TargetNode(startX - 2, startY - 2, -damage, false);
                    break;

                case TargetingType.FullGrid:
                    for (int i = 0; i < grid.Size; i++)
                    {
                        for (int j = 0; j < grid.Size; j++)
                        {
                            grid.TargetNode(i, j, -damage, false);
                        }
                    }
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

                case TargetingType.BigLine:
                    switch (targetingDirection)
                    {
                        case TargetingDirection.Up:
                            for (int i = 0; i < grid.Size; i++)
                            {
                                grid.HighlightNode(startPosition, i, state);
                                grid.HighlightNode(startPosition - 1, i, state);
                                grid.HighlightNode(startPosition + 1, i, state);
                            }
                            break;
                        case TargetingDirection.Down:
                            for (int i = grid.Size - 1; i >= 0; i--)
                            {
                                grid.HighlightNode(startPosition, i, state);
                                grid.HighlightNode(startPosition - 1, i, state);
                                grid.HighlightNode(startPosition + 1, i, state);
                            }
                            break;
                        case TargetingDirection.Left:
                            for (int i = grid.Size - 1; i >= 0; i--)
                            {
                                grid.HighlightNode(i, startPosition, state);
                                grid.HighlightNode(i, startPosition - 1, state);
                                grid.HighlightNode(i, startPosition + 1, state);
                            }
                            break;
                        case TargetingDirection.Right:
                            for (int i = 0; i < grid.Size; i++)
                            {
                                grid.HighlightNode(i, startPosition, state);
                                grid.HighlightNode(i, startPosition - 1, state);
                                grid.HighlightNode(i, startPosition + 1, state);
                            }
                            break;
                    }
                    break;

                case TargetingType.BigSquare:
                    grid.HighlightNode(startX, startY, state);
                    grid.HighlightNode(startX + 1, startY, state);
                    grid.HighlightNode(startX, startY + 1, state);
                    grid.HighlightNode(startX + 1, startY + 1, state);
                    grid.HighlightNode(startX + 1, startY - 1, state);
                    grid.HighlightNode(startX - 1, startY - 1, state);
                    grid.HighlightNode(startX, startY - 1, state);
                    grid.HighlightNode(startX - 1, startY, state);
                    grid.HighlightNode(startX - 1, startY + 1, state);
                    break;

                case TargetingType.BigCross:
                    grid.HighlightNode(startX, startY, state);
                    grid.HighlightNode(startX + 1, startY, state);
                    grid.HighlightNode(startX + 2, startY, state);
                    grid.HighlightNode(startX, startY + 1, state);
                    grid.HighlightNode(startX, startY + 2, state);
                    grid.HighlightNode(startX - 1, startY, state);
                    grid.HighlightNode(startX - 2, startY, state);
                    grid.HighlightNode(startX, startY - 1, state);
                    grid.HighlightNode(startX, startY - 2, state);
                    grid.HighlightNode(startX + 1, startY + 1, state);
                    grid.HighlightNode(startX + 2, startY + 2, state);
                    grid.HighlightNode(startX + 1, startY - 1, state);
                    grid.HighlightNode(startX + 2, startY - 2, state);
                    grid.HighlightNode(startX - 1, startY + 1, state);
                    grid.HighlightNode(startX - 2, startY + 2, state);
                    grid.HighlightNode(startX - 1, startY - 1, state);
                    grid.HighlightNode(startX - 2, startY - 2, state);
                    break;

                case TargetingType.FullGrid:
                    for (int i = 0; i < grid.Size; i++)
                    {
                        for (int j = 0; j < grid.Size; j++)
                        {
                            grid.HighlightNode(i, j, state);
                        }
                    }
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
                                grid.TargetNode(startPosition, i, -damage, false);
                                yield return new WaitForSeconds(0.2f);
                            }
                            break;
                        case TargetingDirection.Down:
                            for (int i = grid.Size - 1; i >= 0; i--)
                            {
                                AttackNode(startPosition, i, damage);
                                grid.TargetNode(startPosition, i, -damage, false);
                                yield return new WaitForSeconds(0.2f);
                            }
                            break;
                        case TargetingDirection.Left:
                            for (int i = grid.Size - 1; i >= 0; i--)
                            {
                                AttackNode(i, startPosition, damage);
                                grid.TargetNode(i, startPosition, -damage, false);
                                yield return new WaitForSeconds(0.2f);
                            }
                            break;
                        case TargetingDirection.Right:
                            for (int i = 0; i < grid.Size; i++)
                            {
                                AttackNode(i, startPosition, damage);
                                grid.TargetNode(i, startPosition, -damage, false);
                                yield return new WaitForSeconds(0.2f);
                            }
                            break;
                    }
                    break;

                case TargetingType.Square:
                    AttackNode(startX, startY, damage);
                    grid.TargetNode(startX, startY, -damage, false);
                    AttackNode(startX + 1, startY, damage);
                    grid.TargetNode(startX + 1, startY, -damage, false);
                    AttackNode(startX, startY + 1, damage);
                    grid.TargetNode(startX, startY + 1, -damage, false);
                    AttackNode(startX + 1, startY + 1, damage);
                    grid.TargetNode(startX + 1, startY + 1, -damage, false);
                    yield return new WaitForSeconds(0.5f);
                    break;

                case TargetingType.Cross:
                    AttackNode(startX, startY, damage);
                    grid.TargetNode(startX, startY, -damage, false);
                    yield return new WaitForSeconds(0.25f);

                    AttackNode(startX + 1, startY, damage);
                    grid.TargetNode(startX + 1, startY, -damage, false);
                    AttackNode(startX, startY + 1, damage);
                    grid.TargetNode(startX, startY + 1, -damage, false);
                    AttackNode(startX - 1, startY, damage);
                    grid.TargetNode(startX - 1, startY, -damage, false);
                    AttackNode(startX, startY - 1, damage);
                    grid.TargetNode(startX, startY - 1, -damage, false);
                    yield return new WaitForSeconds(0.5f);
                    break;

                case TargetingType.BigLine:
                    switch (targetingDirection)
                    {
                        case TargetingDirection.Up:
                            for (int i = 0; i < grid.Size; i++)
                            {
                                AttackNode(startPosition, i, damage);
                                AttackNode(startPosition - 1, i, damage);
                                AttackNode(startPosition + 1, i, damage);
                                grid.TargetNode(startPosition, i, -damage, false);
                                grid.TargetNode(startPosition - 1, i, -damage, false);
                                grid.TargetNode(startPosition + 1, i, -damage, false);
                                yield return new WaitForSeconds(0.2f);
                            }
                            break;
                        case TargetingDirection.Down:
                            for (int i = grid.Size - 1; i >= 0; i--)
                            {
                                AttackNode(startPosition, i, damage);
                                AttackNode(startPosition - 1, i, damage);
                                AttackNode(startPosition + 1, i, damage);
                                grid.TargetNode(startPosition, i, -damage, false);
                                grid.TargetNode(startPosition - 1, i, -damage, false);
                                grid.TargetNode(startPosition + 1, i, -damage, false);
                                yield return new WaitForSeconds(0.2f);
                            }
                            break;
                        case TargetingDirection.Left:
                            for (int i = grid.Size - 1; i >= 0; i--)
                            {
                                AttackNode(i, startPosition, damage);
                                AttackNode(i, startPosition - 1, damage);
                                AttackNode(i, startPosition + 1, damage);
                                grid.TargetNode(i, startPosition, -damage, false);
                                grid.TargetNode(i, startPosition - 1, -damage, false);
                                grid.TargetNode(i, startPosition + 1, -damage, false);
                                yield return new WaitForSeconds(0.2f);
                            }
                            break;
                        case TargetingDirection.Right:
                            for (int i = 0; i < grid.Size; i++)
                            {
                                AttackNode(i, startPosition, damage);
                                AttackNode(i, startPosition - 1, damage);
                                AttackNode(i, startPosition + 1, damage);
                                grid.TargetNode(i, startPosition, -damage, false);
                                grid.TargetNode(i, startPosition - 1, -damage, false);
                                grid.TargetNode(i, startPosition + 1, -damage, false);
                                yield return new WaitForSeconds(0.2f);
                            }
                            break;
                    }
                    break;

                case TargetingType.BigSquare:
                    AttackNode(startX, startY, damage);
                    grid.TargetNode(startX, startY, -damage, false);
                    AttackNode(startX + 1, startY, damage);
                    grid.TargetNode(startX + 1, startY, -damage, false);
                    AttackNode(startX, startY + 1, damage);
                    grid.TargetNode(startX, startY + 1, -damage, false);
                    AttackNode(startX + 1, startY + 1, damage);
                    grid.TargetNode(startX + 1, startY + 1, -damage, false);
                    AttackNode(startX + 1, startY - 1, damage);
                    grid.TargetNode(startX + 1, startY - 1, -damage, false);
                    AttackNode(startX - 1, startY - 1, damage);
                    grid.TargetNode(startX - 1, startY - 1, -damage, false);
                    AttackNode(startX, startY - 1, damage);
                    grid.TargetNode(startX, startY - 1, -damage, false);
                    AttackNode(startX - 1, startY, damage);
                    grid.TargetNode(startX - 1, startY, -damage, false);
                    AttackNode(startX - 1, startY + 1, damage);
                    grid.TargetNode(startX - 1, startY + 1, -damage, false);
                    yield return new WaitForSeconds(0.5f);
                    break;

                case TargetingType.BigCross:
                    AttackNode(startX, startY, damage);
                    grid.TargetNode(startX, startY, -damage, false);
                    yield return new WaitForSeconds(0.25f);

                    AttackNode(startX + 1, startY, damage);
                    grid.TargetNode(startX + 1, startY, -damage, false);
                    AttackNode(startX, startY + 1, damage);
                    grid.TargetNode(startX, startY + 1, -damage, false);
                    AttackNode(startX + 1, startY + 1, damage);
                    grid.TargetNode(startX + 1, startY + 1, -damage, false);
                    AttackNode(startX + 1, startY - 1, damage);
                    grid.TargetNode(startX + 1, startY - 1, -damage, false);
                    AttackNode(startX - 1, startY - 1, damage);
                    grid.TargetNode(startX - 1, startY - 1, -damage, false);
                    AttackNode(startX, startY - 1, damage);
                    grid.TargetNode(startX, startY - 1, -damage, false);
                    AttackNode(startX - 1, startY, damage);
                    grid.TargetNode(startX - 1, startY, -damage, false);
                    AttackNode(startX - 1, startY + 1, damage);
                    grid.TargetNode(startX - 1, startY + 1, -damage, false);
                    yield return new WaitForSeconds(0.25f);

                    AttackNode(startX + 2, startY, damage);
                    grid.TargetNode(startX + 2, startY, -damage, false);
                    AttackNode(startX, startY + 2, damage);
                    grid.TargetNode(startX, startY + 2, -damage, false);
                    AttackNode(startX - 2, startY, damage);
                    grid.TargetNode(startX - 2, startY, -damage, false);
                    AttackNode(startX, startY - 2, damage);
                    grid.TargetNode(startX, startY - 2, -damage, false);
                    AttackNode(startX + 2, startY + 2, damage);
                    grid.TargetNode(startX + 2, startY + 2, -damage, false);
                    AttackNode(startX + 2, startY - 2, damage);
                    grid.TargetNode(startX + 2, startY - 2, -damage, false);
                    AttackNode(startX - 2, startY + 2, damage);
                    grid.TargetNode(startX - 2, startY + 2, -damage, false);
                    AttackNode(startX - 2, startY - 2, damage);
                    grid.TargetNode(startX - 2, startY - 2, -damage, false);
                    yield return new WaitForSeconds(0.5f);
                    break;

                case TargetingType.FullGrid:
                    for (int i = 0; i < grid.Size; i++)
                    {
                        for (int j = 0; j < grid.Size; j++)
                        {
                            AttackNode(i, j, damage);
                            grid.TargetNode(i, j, -damage, false);
                        }
                    }
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
