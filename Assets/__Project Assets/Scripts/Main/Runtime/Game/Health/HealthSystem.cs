using d4160.Core;
using NaughtyAttributes;
using UltEvents;
using UnityEngine;

namespace Game.Health
{
    public class HealthSystem : MonoBehaviourData<Health>
    {
        [Header("EVENTS")]
        public UltEvent<int, int, int, Vector2> OnHPModified;
        public UltEvent OnHPBecomeZero;

        [Button]
        public void TakeDamage()
        {
            TakeDamage(10, Random.insideUnitCircle * 5f);
        }

        public void TakeDamage(int amount, Vector2 position)
        {
            _data.ModifyHP(-amount);

            OnHPModified?.Invoke(-amount, _data.currentHP, _data.maxHP, position);

            if (_data.IsZero)
            {
                OnHPBecomeZero?.Invoke();
            }
        }

        [Button]
        public void RestoreHealth()
        {
            RestoreHealth(10, Random.insideUnitCircle * 5f);
        }

        public void RestoreHealth(int amount, Vector2 position)
        {
            _data.ModifyHP(+amount);

            OnHPModified?.Invoke(+amount, _data.currentHP, _data.maxHP, position);
        }
    }
}
