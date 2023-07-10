using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public enum EnemyType
    {
        Snake = 0,
        Slime = 1,
        Tree = 2
    }

    public enum EnemyDifficulty
    {
        Easy = 0,
        Normal = 1,
        Hard = 2
    }

    [System.Serializable]
    public class EnemyData
    {
        [SerializeField]
        public EnemyType Type;

        [SerializeField]
        public EnemyDifficulty Difficulty;
    }
}
