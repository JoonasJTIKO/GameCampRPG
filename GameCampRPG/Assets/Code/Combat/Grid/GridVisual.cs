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
        private GridNodeVisual nodePrefab;

        [SerializeField]
        private GridNodeVisual targetedNodePrefab;

        [SerializeField]
        private float nodeDistance = 1f;

        private GridNodeVisual[,] grid;
        private GridNodeVisual[,] targetedGrid;

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

            grid = new GridNodeVisual[size, size];
            targetedGrid = new GridNodeVisual[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    GameObject node = Instantiate(nodePrefab.gameObject);
                    node.transform.position = new Vector3(transform.position.x + j * nodeDistance, transform.position.y, transform.position.z + i * nodeDistance);
                    grid[j, i] = node.GetComponent<GridNodeVisual>();

                    GameObject targetedNode = Instantiate(targetedNodePrefab.gameObject);
                    targetedNode.transform.position = new Vector3(transform.position.x + j * nodeDistance, transform.position.y, transform.position.z + i * nodeDistance);
                    targetedGrid[j, i] = targetedNode.GetComponent<GridNodeVisual>();
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

            grid[x, y].Highlight(state);
            targetedGrid[x, y].Highlight(state);
        }

        public Vector3 GetNodePosition(int x, int y)
        {
            Vector3 returnValue = 
                new Vector3(grid[x, y].transform.position.x, 
                grid[x, y].transform.position.y + 1, 
                grid[x, y].transform.position.z);

            return returnValue;
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
            if (x < 0 || x >= Size || y < 0 || y >= Size) return;

            gridData.TargetNode(x, y, state);
            if (state)
            {
                targetedGrid[x, y].gameObject.SetActive(true);
                grid[x, y].gameObject.SetActive(false);
            }
            else if (gridData.Grid[x, y].TargetingCount == 0)
            {
                targetedGrid[x, y].gameObject.SetActive(false);
                grid[x, y].gameObject.SetActive(true);
            }
            targetedGrid[x, y].SetTargetedCount(gridData.Grid[x, y].TargetingCount);
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
