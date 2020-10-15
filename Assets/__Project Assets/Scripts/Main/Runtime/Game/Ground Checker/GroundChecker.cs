using UnityEngine;

namespace Game.Ground_Checker
{
    [System.Serializable]
    public struct GroundChecker
    {
        public Vector2 startPoint;
        public float distance;
        public LayerMask groundLayer;

        public Vector2 EndPoint => startPoint + Vector2.down * distance;
    }
}
