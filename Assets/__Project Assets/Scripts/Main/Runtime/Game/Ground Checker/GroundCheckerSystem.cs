using d4160.Core;
using DG.DemiLib;
using UnityEngine;

namespace Game.Ground_Checker
{
    public class GroundCheckerSystem : MonoBehaviourData<GroundChecker>
    {
        [SerializeField] private Transform _groundCheckPoint;

        public bool CheckGround()
        {
            _data.startPoint = _groundCheckPoint.position;
            return Physics2D.Linecast(_data.startPoint, _data.EndPoint, _data.groundLayer);
        }

        void OnDrawGizmosSelected()
        {
            Debug.DrawLine(_groundCheckPoint.position, _groundCheckPoint.position + Vector3.down * _data.distance, Color.yellow);
        }
    }
}
