// Assets/Scripts/BlockCoding/Combat/CombatSystem.cs
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Diagnostics;

namespace Combat
{
    public class CombatSystem : MonoBehaviour
    {
        [Header("Animation")]
        [Tooltip("공격 애니메이션이 들어있는 Animator 컴포넌트를 할당하세요.")]
        public Animator playerAnimator;

        [Header("Stats")]
        [Tooltip("공격 데미지")]
        public int damage = 10;
        [Tooltip("최대 체력")]
        public int maxHealth = 100;
        private int currentHealth;

        [Header("Attack Effect")]
        [Tooltip("타격 이펙트를 활성화할 GameObject")]
        public GameObject attackPoint;
        [Tooltip("애니메이션 전체에서 이펙트를 켤 시점을 normalized time(0~1)으로 지정")]
        [Range(0f,1f)]
        public float hitTimeNormalized = 0.5f;

        void Awake()
        {
            currentHealth = maxHealth;
            if (attackPoint != null)
                attackPoint.SetActive(false);
        }

        /// <summary>
        /// 공격 애니메이션만 트리거합니다.
        /// </summary>
        public void PerformAttack()
        {
            playerAnimator?.SetTrigger("Attack");
        }

        /// <summary>
        /// 애니메이션 속도까지 반영해 정확히 전체 재생 시간만큼 대기한 뒤 이펙트를 켜고, 남은 시간 후 끕니다.
        /// </summary>
        public async UniTask PlayAttackEffectAsync()
        {
            if (playerAnimator == null)
                return;

            // 재생 속도 반영
            float speed = Mathf.Max(0.01f, playerAnimator.speed);
            var clips   = playerAnimator.runtimeAnimatorController?.animationClips;
            var clip    = clips?.FirstOrDefault(c => c.name.Contains("Attack"));

            float baseMs   = clip != null ? clip.length * 1000f : 300f;
            int   fullMs   = Mathf.CeilToInt(baseMs / speed);
            int   hitMs    = Mathf.FloorToInt(fullMs * hitTimeNormalized);

            UnityEngine.Debug.Log(
                $"[CombatSystem] PlayAttackEffectAsync 호출 — speed: {speed:0.00}, " +
                $"풀타임: {fullMs}ms, 피격지연: {hitMs}ms"
            );

            // 애니메이션 시작
            playerAnimator.SetTrigger("Attack");

            // 타이밍 측정
            var sw = Stopwatch.StartNew();

            // 피격 시점까지 대기
            await UniTask.Delay(hitMs, DelayType.DeltaTime);
            attackPoint?.SetActive(true);

            // 남은 시간 대기
            await UniTask.Delay(fullMs - hitMs, DelayType.DeltaTime);
            attackPoint?.SetActive(false);

            sw.Stop();
            UnityEngine.Debug.Log(
                $"[CombatSystem] 이펙트 완료 — 실제 대기: {sw.ElapsedMilliseconds}ms"
            );
        }

        /// <summary>
        /// 방어 애니메이션 트리거
        /// </summary>
        public void PerformDefend()
        {
            playerAnimator?.SetTrigger("Defend");
        }

        /// <summary>
        /// 데미지 처리
        /// </summary>
        public void TakeDamage(int amount)
        {
            currentHealth = Mathf.Max(0, currentHealth - amount);
            if (currentHealth <= 0)
                Destroy(gameObject);
        }
    }
}
