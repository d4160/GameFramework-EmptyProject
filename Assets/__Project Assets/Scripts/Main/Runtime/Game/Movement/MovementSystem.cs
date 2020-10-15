using d4160.Core;
using Game.Character;
using Game.Ground_Checker;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementSystem : MonoBehaviourData<Movement>
    {
        [SerializeField] private GroundCheckerSystem _groundChecker;

        private CharacterAnimator _animator;
        private Rigidbody2D _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            float yVelocity = _rb.velocity.y;
            _rb.velocity = new Vector2(_data.direction * _data.movementSpeed, yVelocity);

            if (!_animator)
            {
                _animator = GetComponentInChildren<CharacterAnimator>();
            }

            _animator.SetSpeed((Mathf.Abs(_rb.velocity.x / _data.movementSpeed)));
        }

        [Button]
        public void Jump()
        {
            if (!_groundChecker)
            {
                _groundChecker = GetComponentInChildren<GroundCheckerSystem>();
            }

            if(!_rb || !_data.canJump || !_groundChecker || !_groundChecker.CheckGround())
                return;
            
            _rb.AddForce(Vector2.up * _data.jumpImpulse, ForceMode2D.Impulse);
        }

        public void ChangeDirection(int newDirection)
        {
            _data.direction = newDirection;
        }
    }
}
