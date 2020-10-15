using UnityEngine;

namespace Game.Combat
{
    [System.Serializable]
    public struct Defense
    {
        public int armor;
        [Range(0, 1f)]
        public float blockProbability;
        [Range(0,1f)]
        public float dodgeProbability;
        [Range(0,1f)]
        public float damageReduction;
    }
}