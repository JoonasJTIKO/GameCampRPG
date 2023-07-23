using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCampRPG
{
    public class UnitShake : MonoBehaviour
    {
        [SerializeField]
        private float shakesPerSecond = 8;

        private Coroutine shakeRoutine;

        public void Shake(float duration)
        {
            if (shakeRoutine != null) StopCoroutine(shakeRoutine);
            shakeRoutine = StartCoroutine(ShakeRoutine(duration));
        }

        private IEnumerator ShakeRoutine(float duration)
        {
            Vector3 startPos = transform.position;
            float deltaTime = 0;

            while (deltaTime <= duration)
            {
                deltaTime += Time.deltaTime;
                transform.position = new Vector3(transform.position.x, transform.position.y, startPos.z + Mathf.Sin(deltaTime * shakesPerSecond) * 0.3f);
                yield return null;
            }
            transform.position = startPos;
        }
    }
}
