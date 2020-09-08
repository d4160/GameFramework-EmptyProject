using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class LayerTest : MonoBehaviour
    {
        [Layer]
        public int layer0;

        public LayerNest1 nest1;

        [Button]
        private void LogTag0()
        {
            Debug.Log(layer0);
        }
    }

    [System.Serializable]
    public class LayerNest1
    {
        [Layer]
        public int layer1;

        public LayerNest2 nest2;
    }

    [System.Serializable]
    public struct LayerNest2
    {
        [Layer]
        public int layer2;
    }
}