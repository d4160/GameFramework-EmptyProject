using UnityEngine;
using System.Collections.Generic;

namespace NaughtyAttributes.Test
{
    public class SearchableEnumTest : MonoBehaviour
    {
        [SearchableEnum]
        public KeyCode key1;

        public SearchableEnumNest1 nest1;
    }

    [System.Serializable]
    public class SearchableEnumNest1
    {
        [SearchableEnum]
        public KeyCode key2;

        public SearchableEnumNest2 nest2;
    }

    [System.Serializable]
    public class SearchableEnumNest2
    {
        [SearchableEnum]
        public KeyCode key3;
    }
}