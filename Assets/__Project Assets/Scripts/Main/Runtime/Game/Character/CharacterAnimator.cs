using Game.Health;
using UnityEngine;

namespace Game.Character
{
    [RequireComponent(typeof(CharacterAnimator))]
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private AudioSource _audio;
        [SerializeField] private Transform _gloveBackPoint;
        [SerializeField] private Transform _gloveFrontPoint;
        [SerializeField] private Transform _bootBackPoint;
        [SerializeField] private Transform _bootFrontPoint;
        [SerializeField] private float _gloveBackDistance;
        [SerializeField] private float _gloveFrontDistance;
        [SerializeField] private float _bootBackDistance;
        [SerializeField] private float _bootFrontDistance;

        private Animator _anim;
        private int _damageFinal;
        private Vector3 _attackPoint;
        private float _attackDistance;
        private int _selectedIndex;

        void Start()
        {
            _anim = GetComponent<Animator>();
        }

        public void PlayAttackAnim(int damageFinal, int index)
        {
            string trigger = "";
            _selectedIndex = index;

            switch (index)
            {
                case 1:
                    trigger = "Punch1";
                    _attackPoint = _gloveBackPoint.position;
                    _attackDistance = _gloveBackDistance;
                    break;
                case 2:
                    trigger = "Punch2";
                    _attackPoint = _gloveFrontPoint.position;
                    _attackDistance = _gloveFrontDistance;
                    break;
                case 3:
                    trigger = "Kick1";
                    _attackPoint = _bootBackPoint.position;
                    _attackDistance = _bootBackDistance;
                    break;
                case 4:
                    trigger = "Kick2";
                    _attackPoint = _bootFrontPoint.position;
                    _attackDistance = _bootFrontDistance;
                    break;
            }

            _anim.SetTrigger(trigger);

            _damageFinal = damageFinal;
        }

        public void AttackAction()
        {
            switch (_selectedIndex)
            {
                case 1:
                    _attackPoint = _gloveBackPoint.position;
                    _attackDistance = _gloveBackDistance;
                    break;
                case 2:
                    _attackPoint = _gloveFrontPoint.position;
                    _attackDistance = _gloveFrontDistance;
                    break;
                case 3:
                    _attackPoint = _bootBackPoint.position;
                    _attackDistance = _bootBackDistance;
                    break;
                case 4:
                    _attackPoint = _bootFrontPoint.position;
                    _attackDistance = _bootFrontDistance;
                    break;
            }

            var direction = transform.parent.localScale.x;
            var hit = Physics2D.Raycast(_attackPoint, Vector2.right * direction, _attackDistance);

            if (hit.collider != null)
            {
                // Collides with something
                Vector3 hitPoint = hit.point;
                hit.rigidbody.GetComponent<HealthSystem>().TakeDamage(_damageFinal, hitPoint);

                Debug.Log($"Damage {_damageFinal}");
            }
        }

        public void SetSpeed(float value)
        {
            _anim.SetFloat("Speed", value);
        }

        public void SetVolume(float volume)
        {
            _audio.volume = volume;
        }
    }
}
