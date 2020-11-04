using UnityEngine;

namespace d4160.Core.MonoBehaviours.ObjectCollections
{
    public class UnityObjectItem<T> : MonoBehaviour where T : Object
    {
        public IUnityObjectList<T> List { get; set; }

        public virtual void Remove()
        {
            List?.Remove(this as T);
        }
    }
}
