using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class CombatPlayerMoving : MonoBehaviour
    {
        [SerializeField]
        private int xPosition;

        [SerializeField]
        private int yPosition;

        private GridVisual grid;

        public event Action Moved;

        public int GridPositionX
        {
            get;
            private set;
        }

        public int GridPositionY
        {
            get;
            private set;
        }

        private void Awake()
        {
            grid = FindObjectOfType<GridVisual>();

            GridPositionX = xPosition;
            GridPositionY = yPosition;
        }

        private void Start()
        {
            grid.MoveToNode(GridPositionX, GridPositionY);
        }

        public void MoveToPosition(int x, int y)
        {
            grid.ClearNode(GridPositionX, GridPositionY);
            GridPositionX = x;
            GridPositionY = y;
            StartCoroutine(Move(grid.GetNodePosition(x, y)));
        }

        private IEnumerator Move(Vector3 position)
        {
            transform.position = position;
            yield return new WaitForSeconds(1);
            Moved?.Invoke();
        }
    }
}
