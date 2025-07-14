using UnityEngine;
using Cysharp.Threading.Tasks;  // UniTask 네임스페이스

namespace Combat
{
    public class CombatSystem : MonoBehaviour
    {
        [Header("Animator & Stats")]
        public Animator playerAnimator;
        public int damage = 10;
        public int shield = 5;

        [Header("Health")]
        public int maxHealth = 100;
        private int currentHealth;

        [Header("Attack Point (Trigger Collider)")]
        [SerializeField]
        private GameObject attackPoint;
        [Tooltip("attackPoint를 켜둘 시간(초)")]
        public float attackDuration = 0.2f;

        private void Start()
        {
            currentHealth = maxHealth;
            // if (attackPoint != null)
            //     attackPoint.SetActive(false);
        }

        public void PerformAttack()
        {
            // if (playerAnimator != null)
            //     playerAnimator.SetTrigger("Attack");

            if (attackPoint != null)
                PlayAttackEffectAsync().Forget();
        }

        private async UniTask PlayAttackEffectAsync()
        {
            attackPoint.SetActive(true);

            await UniTask.Delay(
                (int)(attackDuration * 1000),
                DelayType.DeltaTime
            );

            attackPoint.SetActive(false);
        }

        public void PerformDefend()
        {
            if (playerAnimator != null)
                playerAnimator.SetTrigger("Defend");
            Debug.Log($"방어: {shield} 방어력 적용");
        }

        public void TakeDamage(int amount)
        {
            currentHealth -= amount;
            if (currentHealth < 0) currentHealth = 0;
            Debug.Log($"{name} 남은 체력: {currentHealth}");
            
            if (currentHealth <= 0)
            {
                Die();
            }
        }
        
        private void Die()
        {
            // 사망 이펙트, 사운드 재생
            Destroy(gameObject);
        }

        public bool IsDead() => currentHealth <= 0;
        public int  GetCurrentHealth() => currentHealth;
    }
}