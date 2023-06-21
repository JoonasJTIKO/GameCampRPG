using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class FollowCamera2D : MonoBehaviour
    {
        [SerializeField]
        private float minX, maxX, minY, maxY;

        [SerializeField]
        private PlayerUnit player;

        private Vector2 deltaPosition;

        private void Update()
        {
            Vector2 targetPosition = player.transform.position;

            deltaPosition = 
                new Vector2(targetPosition.x - transform.position.x, targetPosition .y - transform.position.y);

            Vector2 newPosition = (Vector2)transform.position + deltaPosition;

            newPosition = 
                new Vector2(Mathf.Clamp(newPosition.x, minX, maxX), Mathf.Clamp(newPosition.y, minY, maxY));

            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }
    }
}
