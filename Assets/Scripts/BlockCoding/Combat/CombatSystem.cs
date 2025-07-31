using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Combat
{
    public class CombatSystem : MonoBehaviour
    {
        [Header("Animator & Stats")]
        public Animator animator;
        public int damage = 10;
        public int shield = 5;

        [Header("Health")]
        public int maxHealth = 100;
        private int currentHealth;

        [Header("Attack Point (Trigger Collider)")]
        [SerializeField] private GameObject attackPoint;
        [Tooltip("attackPoint를 켜둘 시간(초)")]
        public float attackDuration = 0.2f;

        [Header("Attack Timing")]
        [Tooltip("애니메이션 진행도(normalizedTime) 기준 히트 시점 (0~1)")]
        [Range(0f, 1f)] public float hitTimeNormalized = 0.5f;

        [Header("Particle Effect")]
        [SerializeField] private ParticleSystem attackEffect;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            currentHealth = maxHealth;
            if (attackPoint != null)
                attackPoint.SetActive(false);
        }

        public void PerformAttack()
        {
            if (animator != null)
                animator.SetTrigger("Attack");
        }

        public async UniTask PlayAttackEffectAsync()
        {
            Debug.Log("1");
            await UniTask.WaitUntil(() =>
            {
                var info = animator.GetCurrentAnimatorStateInfo(0);
                return info.IsName("Attack");
            });

            await UniTask.WaitUntil(() =>
            {
                var info = animator.GetCurrentAnimatorStateInfo(0);
                return info.IsName("Attack")
                       && info.normalizedTime >= hitTimeNormalized;
            });

            attackPoint.SetActive(true);

            await UniTask.Delay((int)(attackDuration * 1000), DelayType.DeltaTime);
            attackPoint.SetActive(false);
        }

        public void PerformDefend()
        {
            if (animator != null)
                animator.SetTrigger("Defend");
            Debug.Log($"방어: {shield} 방어력 적용");
        }

        public void TakeDamage(int amount)
        {
            currentHealth -= amount;
            if (currentHealth < 0) currentHealth = 0;
            Debug.Log($"{name} 남은 체력: {currentHealth}");

            if (currentHealth <= 0)
                Die();
        }

        private void Die()
        {
            Destroy(gameObject);
        }

        public bool IsDead() => currentHealth <= 0;
        public int GetCurrentHealth() => currentHealth;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy"))
                return;

            var enemy = other.GetComponent<CombatSystem>();
            if (enemy != null && !enemy.IsDead())
                enemy.TakeDamage(damage);
        }
    }
}
