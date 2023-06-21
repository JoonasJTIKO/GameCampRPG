using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class UnitBase : MonoBehaviour
    {
        [SerializeField]
        protected float speed;

        protected Mover mover;

        protected virtual void Awake()
        {
            mover = GetComponent<Mover>();
            mover.Setup(speed);
        }
    }
}
