using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class SortLayerTest : MonoBehaviour
    {
        [SortLayer]
        public int spriteLayer0;

        public SortLayerNest1 nest1;

        [Button]
        private void LogSortLayer0()
        {
            Debug.Log(spriteLayer0);
        }
    }

    [System.Serializable]
    public class SortLayerNest1
    {
        [SortLayer]
        public int spriteLayer1;

        public SortLayerNest2 nest2;
    }

    [System.Serializable]
    public struct SortLayerNest2
    {
        [SortLayer]
        public int spriteLayer2;
    }
}