using UnityEngine;
using System.Collections;

namespace Movement
{
    [RequireComponent(typeof(Transform))]
    public class MovementSystem : MonoBehaviour
    {
        [Header("Move Settings")]
        public float moveDistance   = 1f;
        public float moveDuration   = 0.2f;

        [Header("Rotate Settings")]
        public float rotationAngle  = 90f;
        public float rotationDuration = 0.2f;

        public IEnumerator PerformMove()
        {
            Vector3 start = transform.position;
            Vector3 end   = start + transform.forward * moveDistance;
            float t = 0f;
            while (t < moveDuration)
            {
                transform.position = Vector3.Lerp(start, end, t / moveDuration);
                t += Time.deltaTime;
                yield return null;
            }
            transform.position = end;
        }

        public IEnumerator PerformRotate(float angle)
        {
            Quaternion start = transform.rotation;
            Quaternion end   = start * Quaternion.Euler(0, angle, 0);
            float t = 0f;
            while (t < rotationDuration)
            {
                transform.rotation = Quaternion.Slerp(start, end, t / rotationDuration);
                t += Time.deltaTime;
                yield return null;
            }
            transform.rotation = end;
        }
    }
}