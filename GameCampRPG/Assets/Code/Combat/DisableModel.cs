using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class DisableModel : MonoBehaviour
    {
        [SerializeField]
        private GameObject model;

        public void Disable()
        {
            model.SetActive(false);
        }
    }
}
