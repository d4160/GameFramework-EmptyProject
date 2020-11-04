using d4160.Core;
using Game._Camera;
using Game._Player;
using UnityEngine;

namespace Game.Rotation
{
    public class LookAtSystem : MonoBehaviourData<LookAt>
    {
        private Transform _target;
        private Transform _cachedTransform;

        void Start()
        {
            _cachedTransform = transform;
        }

        void Update()
        {
            if (!_target)
            {
                _target = Camera.main.transform;
            }

            _cachedTransform.LookAt(_target, _data.worldUp);

            var eulerAngles = _cachedTransform.localEulerAngles;
            eulerAngles.z = 0;
            _cachedTransform.localEulerAngles = eulerAngles;
        }
    }
}
