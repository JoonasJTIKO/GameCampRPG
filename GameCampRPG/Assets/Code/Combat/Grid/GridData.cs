using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class GridData
    {
        public GridNode[,] Grid
        {
            get;
            private set;
        }

        public void CreateGrid(int size)
        {
            Grid = new GridNode[size, size];

            for (int i = 0; i < Grid.GetLength(0); i++)
            {
                for (int j = 0; j < Grid.GetLength(1); j++)
                {
                    Grid[i, j] = new GridNode();
                }
            }
        }

        public bool MoveToNode(int x, int y)
        {
            if (x < 0 || y < 0 || x > Grid.GetLength(0) || y > Grid.GetLength(1))
            {
                Debug.LogWarning("Attempting to move to node outside the grid!");
                return false;
            }

            if (Grid[x, y].Occupied) return false;

            Grid[x, y].Occupied = true;
            return true;
        }

        public void ClearNode(int x, int y)
        {
            if (x < 0 || y < 0 || x > Grid.GetLength(0) || y > Grid.GetLength(1))
            {
                Debug.LogWarning("Attempting to clear node outside the grid!");
                return;
            }

            Grid[x, y].Occupied = false;
        }

        public void TargetNode(int x, int y, bool state = true)
        {
            if (state) Grid[x, y].TargetingCount++;
            else Grid[x, y].TargetingCount--;
        }
    }
}
