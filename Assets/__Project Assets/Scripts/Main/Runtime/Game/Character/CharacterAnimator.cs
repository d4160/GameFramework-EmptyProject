using Game.Health;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Character
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimator : MonoBehaviour
    {
        [AnimatorParam("Animator")]
        [SerializeField] private int _forwardSpeedParam;
        [AnimatorParam("Animator")]
        [SerializeField] private int _verticalSpeedParam;
        [AnimatorParam("Animator")]
        [SerializeField] private int _lateralSpeedParam;
        [AnimatorParam("Animator")]
        [SerializeField] private int _turningSpeedParam;
        [AnimatorParam("Animator")]
        [SerializeField] private int _onRightFootParam;
        [AnimatorParam("Animator")]
        [SerializeField] private int _jumpedParam;
        [AnimatorParam("Animator")]
        [SerializeField] private int _fallParam;
        [AnimatorParam("Animator")]
        [SerializeField] private int _strafeParam;
        [AnimatorParam("Animator")]
        [SerializeField] private int _speedMultiplierParam;

        private Animator _anim;

        public Animator Animator
        {
            get
            {
                if (!_anim)
                    _anim = GetComponent<Animator>();
                return _anim;
            }
        }

        void Start()
        {
            _anim = GetComponent<Animator>();
        }

        public float ForwardSpeed
        {
            get => _anim.GetFloat(_forwardSpeedParam);
            set => _anim.SetFloat(_forwardSpeedParam, value);
        }

        public float VerticalSpeed
        {
            get => _anim.GetFloat(_verticalSpeedParam);
            set => _anim.SetFloat(_verticalSpeedParam, value);
        }

        public float LateralSpeed
        {
            get => _anim.GetFloat(_lateralSpeedParam);
            set => _anim.SetFloat(_lateralSpeedParam, value);
        }

        public float TurningSpeed
        {
            get => _anim.GetFloat(_turningSpeedParam);
            set => _anim.SetFloat(_turningSpeedParam, value);
        }

        public bool OnRightFoot
        {
            get => _anim.GetBool(_onRightFootParam);
            set => _anim.SetBool(_onRightFootParam, value);
        }

        public void JumpedTrigger()
        {
            _anim.SetTrigger(_jumpedParam);
        }

        public void FallTrigger()
        {
            _anim.SetTrigger(_fallParam);
        }

        public bool Strafe
        {
            get => _anim.GetBool(_strafeParam);
            set => _anim.SetBool(_strafeParam, value);
        }

        public float SpeedMultiplier
        {
            get => _anim.GetFloat(_speedMultiplierParam);
            set => _anim.SetFloat(_speedMultiplierParam, value);
        }
    }
}
