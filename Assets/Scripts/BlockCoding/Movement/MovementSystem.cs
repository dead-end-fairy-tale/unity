using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Movement
{
    public class MovementSystem : MonoBehaviour
    {
        public float moveDistance   = 1f;
        public float moveDuration   = 0.2f;
        public float rotationAngle   = 90f;
        public float rotationDuration = 0.2f;

        public async UniTask PerformMoveAsync()
        {
            Vector3 start = transform.position;
            Vector3 end   = start + transform.forward * moveDistance;
            float t = 0f;
            while (t < moveDuration)
            {
                transform.position = Vector3.Lerp(start, end, t / moveDuration);
                t += Time.deltaTime;
                await UniTask.Yield();
            }
            transform.position = end;
        }

        public async UniTask PerformRotateAsync(float angle)
        {
            Quaternion start = transform.rotation;
            Quaternion end   = start * Quaternion.Euler(0f, angle, 0f);
            float t = 0f;
            while (t < rotationDuration)
            {
                transform.rotation = Quaternion.Slerp(start, end, t / rotationDuration);
                t += Time.deltaTime;
                await UniTask.Yield();
            }
            transform.rotation = end;
        }
    }
}