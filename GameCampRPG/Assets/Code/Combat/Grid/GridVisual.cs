using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class GridVisual : MonoBehaviour
    {
        [SerializeField]
        private List<CombatPlayerMoving> playerUnits;

        [SerializeField]
        private GameObject nodePrefab;

        [SerializeField]
        private GameObject targetedNodePrefab;

        [SerializeField]
        private float nodeDistance = 1f;

        private GameObject[,] grid;
        private GameObject[,] targetedGrid;

        private GridData gridData;

        public int Size
        {
            get;
            private set;
        }

        private void Awake()
        {
            CreateGrid(5);
        }

        public void CreateGrid(int size)
        {
            if (grid != null) return;

            grid = new GameObject[size, size];
            targetedGrid = new GameObject[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    GameObject node = Instantiate(nodePrefab);
                    node.transform.position = new Vector3(transform.position.x + j * nodeDistance, transform.position.y, transform.position.z + i * nodeDistance);
                    grid[j, i] = node;

                    GameObject targetedNode = Instantiate(targetedNodePrefab);
                    targetedNode.transform.position = new Vector3(transform.position.x + j * nodeDistance, transform.position.y, transform.position.z + i * nodeDistance);
                    targetedGrid[j, i] = targetedNode;
                    targetedNode.SetActive(false);
                }
            }

            gridData = new GridData();
            gridData.CreateGrid(size);
            Size = size;
        }

        public void HighlightNode(int x, int y, bool state)
        {
            if (x < 0 || x >= Size || y < 0 || y >= Size) return;

            if (state)
            {
                grid[x, y].transform.localScale = new Vector3(1, 1, 1);
                targetedGrid[x, y].transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                grid[x, y].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                targetedGrid[x, y].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            }
        }

        public Vector3 GetNodePosition(int x, int y)
        {
            return grid[x, y].transform.position;
        }

        public bool MoveToNode(int x, int y)
        {
            return gridData.MoveToNode(x, y);
        }

        public void ClearNode(int x, int y)
        {
            gridData.ClearNode(x, y);
        }

        public void TargetNode(int x, int y, bool state = true)
        {
            gridData.TargetNode(x, y, state);
            if (state)
            {
                targetedGrid[x, y].SetActive(true);
                grid[x, y].SetActive(false);
            }
            else if (gridData.Grid[x, y].TargetingCount == 0)
            {
                targetedGrid[x, y].SetActive(false);
                grid[x, y].SetActive(true);
            }
        }

        public GameObject CheckForPlayer(int x, int y)
        {
            foreach (CombatPlayerMoving unit in playerUnits)
            {
                if (unit.GridPositionX == x && unit.GridPositionY == y)
                {
                    return unit.gameObject;
                }
            }
            return null;
        }
    }
}
