using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class CombatCameraMoving : MonoBehaviour
    {
        [SerializeField]
        private Transform[] cameraSpots;

        [SerializeField]
        private float transitionDuration = 1f;

        private Coroutine moveRoutine = null;

        public enum CameraPosition
        {
            Default,
            Grid,
            Enemies,
            Attack
        }

        public void MoveCamera(CameraPosition targetPosition)
        {
            if (moveRoutine != null) StopCoroutine(moveRoutine);

            switch (targetPosition)
            {
                case CameraPosition.Default:
                    moveRoutine = StartCoroutine(MoveTo(cameraSpots[0].transform.position, cameraSpots[0].transform.rotation));
                    break;
                case CameraPosition.Grid:
                    moveRoutine = StartCoroutine(MoveTo(cameraSpots[1].transform.position, cameraSpots[1].transform.rotation));
                    break;
                case CameraPosition.Enemies:
                    moveRoutine = StartCoroutine(MoveTo(cameraSpots[2].transform.position, cameraSpots[2].transform.rotation));
                    break;
                case CameraPosition.Attack:
                    moveRoutine = StartCoroutine(MoveTo(cameraSpots[3].transform.position, cameraSpots[3].transform.rotation));
                    break;
            }
        }

        private IEnumerator MoveTo(Vector3 targetPosition, Quaternion targetRotation)
        {
            float timer = 0f;
            Vector3 startPosition = transform.position;
            Quaternion startRotation = transform.rotation;

            while (timer < transitionDuration)
            {
                timer += Time.deltaTime;
                transform.position = Vector3.Lerp(startPosition, targetPosition, timer / transitionDuration);
                transform.rotation = Quaternion.Lerp(startRotation, targetRotation, timer / transitionDuration);
                yield return null;
            }
        }
    }
}
