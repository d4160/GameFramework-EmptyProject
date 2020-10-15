using UnityEngine;

namespace Game.Health
{
    [System.Serializable]
    public struct Health
    {
        public int maxHP;
        public int currentHP;

        public bool IsZero => currentHP == 0;

        public void ModifyHP(int value)
        {
            currentHP = Mathf.Clamp(currentHP + value, 0, maxHP);
        }
    }
}
