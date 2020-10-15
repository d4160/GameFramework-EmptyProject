
using UnityEngine;

namespace Game.Combat
{
    [System.Serializable]
    public struct Damage
    {
        public int damage;
        public float attackSpeed;
        public float attacksBySecond;
        [Range(0,1f)]
        public float hitProbability;
        [Range(0, 1f)]
        public float criticalHitProbability;
        [Range(0, 1f)]
        public float stunProbability;
        [Range(0,1f)]
        public float stunDuration;
        [Range(0,1f)]
        public float damageAmplification;
    }
}