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

        [SerializeField]
        private float moveTime = 0.5f;

        private GridVisual grid;

        private float deltaTime;

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

        private void OnEnable()
        {
            GetComponent<CombatPlayerUnit>().OnDied += PlayerDied;
        }

        private void OnDisable()
        {
            GetComponent<CombatPlayerUnit>().OnDied -= PlayerDied;
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
            Vector3 startPos = transform.position;

            while (deltaTime <= moveTime)
            {
                deltaTime += Time.deltaTime;
                transform.position = Parabola.MParabola(startPos, position, 2f, deltaTime / moveTime);
                yield return null;
            }
            Moved?.Invoke();
        }

        private void PlayerDied()
        {
            grid.ClearNode(GridPositionX, GridPositionY);
        }
    }
}
