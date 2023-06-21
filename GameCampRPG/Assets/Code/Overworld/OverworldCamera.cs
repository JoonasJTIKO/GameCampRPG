using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class OverworldCamera : MonoBehaviour
    {
        private void Start()
        {
            if (GameInstance.Instance == null) return;

            Camera camera = GetComponent<Camera>();
            GameInstance.Instance.SetOverworldCamera(camera);

            if (camera.enabled == false)
            {
                GameInstance.Instance.ActivateOverworldCamera(true);
            }
        }
    }
}
