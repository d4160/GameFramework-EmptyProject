using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dreamteck.Blenda.Editor
{
    public class EditorSerializeObject : ScriptableObject
    {
        public int selection = -1;
        public bool edit = false;
    }
}
