using UnityEngine;
using System.Collections.Generic;

namespace NaughtyAttributes.Test
{
    public class DropdownIndexTest : MonoBehaviour
    {
        [Dropdown("intValues")]
        public int intValue;

#pragma warning disable 414
        private int[] intValues = new int[] { 1, 2, 3 };
#pragma warning restore 414

        public DropdownIndexNest1 nest1;
    }

    [System.Serializable]
    public class DropdownIndexNest1
    {
        [DropdownIndex("StringValues")]
        public int stringValue;

        private List<string> StringValues { get { return new List<string>() { "A", "B", "C" }; } }

        public DropdownIndexNest2 nest2;
    }

    [System.Serializable]
    public class DropdownIndexNest2
    {
        [DropdownIndex("GetVectorValues")]
        public int vectorValue;

        private DropdownList<Vector3> GetVectorValues()
        {
            return new DropdownList<Vector3>()
            {
                { "Right", Vector3.right },
                { "Up", Vector3.up },
                { "Forward", Vector3.forward }
            };
        }
    }
}