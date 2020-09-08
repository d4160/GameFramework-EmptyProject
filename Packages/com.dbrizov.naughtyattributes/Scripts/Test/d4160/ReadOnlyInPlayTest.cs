using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class ReadOnlyInPlayTest : MonoBehaviour
    {
        [ReadOnlyInPlay]
        public int readOnlyInt = 5;

        public ReadOnlyInPlayNest1 nest1;
    }

    [System.Serializable]
    public class ReadOnlyInPlayNest1
    {
        [ReadOnlyInPlay]
        public float readOnlyFloat = 3.14f;

        public ReadOnlyInPlayNest2 nest2;
    }

    [System.Serializable]
    public struct ReadOnlyInPlayNest2
    {
        [ReadOnlyInPlay]
        public string readOnlyString;
    }
}