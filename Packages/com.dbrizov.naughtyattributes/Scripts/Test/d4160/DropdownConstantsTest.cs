using UnityEngine;
using System.Collections.Generic;

namespace NaughtyAttributes.Test
{
    public class DropdownConstantsTest : MonoBehaviour
    {
        [DropdownConstants(typeof(Vector3))]
        public Vector3 v3Value;

        public DropdownConstantsNest1 nest1;
    }

    [System.Serializable]
    public class DropdownConstantsNest1
    {
        [DropdownConstants(typeof(Vector2))]
        public Vector2 v2Value;

        public DropdownConstantsNest2 nest2;
    }

    [System.Serializable]
    public class DropdownConstantsNest2
    {
        [DropdownConstants(typeof(int))]
        public int intValue;
    }
}