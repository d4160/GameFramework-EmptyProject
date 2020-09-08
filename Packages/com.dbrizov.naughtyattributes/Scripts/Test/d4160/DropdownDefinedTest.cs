using UnityEngine;
using System.Collections.Generic;
using System.Globalization;

namespace NaughtyAttributes.Test
{
    public class DropdownDefinedTest : MonoBehaviour
    {
        [DropdownDefined(1, 2, 3, 4, 5)]
        public int intValue;

        public DropdownDefinedNest1 nest1;
    }

    [System.Serializable]
    public class DropdownDefinedNest1
    {
        [DropdownDefined("Val1", "Val2", "V3")]
        public string stringValue;

        public DropdownDefinedNest2 nest2;
    }

    [System.Serializable]
    public class DropdownDefinedNest2
    {
        [DropdownDefined(0.5f, 0.6f, 1f)]
        public float vectorValue;
    }
}