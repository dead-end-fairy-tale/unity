using UnityEngine;

namespace Combat
{
    public class CombatSystem : MonoBehaviour
    {
        [Header("Animator & Stats")]
        public Animator playerAnimator;
        public int damage = 10;
        public int shield  = 5;

        public void PerformAttack()
        {
            if (playerAnimator != null)
                playerAnimator.SetTrigger("Attack");

            // TODO: 실제 적 체력 감소 로직 호출
            Debug.Log($"Dealt {damage} damage.");
        }

        public void PerformDefend()
        {
            if (playerAnimator != null)
                playerAnimator.SetTrigger("Defend");

            // TODO: 방어 버프 적용 로직 호출
            Debug.Log($"Gained {shield} shield.");
        }
    }
}