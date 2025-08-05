// Assets/Scripts/BlockCoding/Combat/AttackPoint.cs
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(Collider))]
    public class AttackPoint : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particle;
        
        private CombatSystem _combatSystem;

        private void Awake()
        {
            _combatSystem = GetComponentInParent<CombatSystem>();
            if (_combatSystem == null)
                Debug.LogError("AttackPoint: 상위 오브젝트에 CombatSystem이 없습니다.");
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Trigger Enter {other.name}");

            var target = other.GetComponent<CombatSystem>();
            if (target != null && _combatSystem != null)
            {
                int dmg = _combatSystem.damage;

                if (_particle != null)
                    _particle.Play();

                target.TakeDamage(dmg);
                Debug.Log($"{other.name}에게 {dmg}만큼 데미지!");
            }
        }
    }
}