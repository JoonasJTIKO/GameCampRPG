using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public enum EnemyType
    {
        Snake = 0,
    }

    [System.Serializable]
    public class EnemyData
    {
        [SerializeField]
        public EnemyType Type;
    }
}
