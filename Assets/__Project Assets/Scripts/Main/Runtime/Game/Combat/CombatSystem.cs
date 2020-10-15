using d4160.Core;
using Game.Character;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Combat
{
    public class CombatSystem : MonoBehaviourData<Damage, Defense>
    {
        private float _attackCountdown;
        private CharacterAnimator _animator;

        public bool CanAttack => _attackCountdown == 0;

        void Update()
        {
            if (_attackCountdown > 0)
            {
                _attackCountdown -= Time.deltaTime;

                if (_attackCountdown < 0)
                    _attackCountdown = 0;
            }
        }

        public void Attack(int index)
        {
            if(!CanAttack)
                return;

            if (!_animator)
            {
                _animator = GetComponentInChildren<CharacterAnimator>();
            }

            var damageFinal = CalculateDamage();

            _animator.PlayAttackAnim(damageFinal, index);

            _attackCountdown = 1f / _data1.attacksBySecond;
        }

        private int CalculateDamage()
        {
            return _data1.damage;
        }
    }
}
