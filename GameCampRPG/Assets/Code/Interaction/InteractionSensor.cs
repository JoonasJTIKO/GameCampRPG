using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class InteractionSensor : MonoBehaviour
    {
        public IInteractable IntersectingObject
        {
            get;
            private set;
        }

        private int collisionCount = 0;

        public bool IsActive
        {
            get { return collisionCount > 0; }
        }

        private void OnTriggerEnter(Collider other)
        {
            collisionCount++;
            IntersectingObject = other.GetComponent<IInteractable>();
            IntersectingObject.PlayerEnterRange(true);
        }

        private void OnTriggerExit(Collider other)
        {
            collisionCount--;

            if (!IsActive)
            {
                IntersectingObject.PlayerEnterRange(false);
                IntersectingObject = null;
            }
        }
    }
}
