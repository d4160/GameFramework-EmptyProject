using UnityEngine;

namespace d4160.Core.MonoBehaviours.ObjectCollections
{
    public interface IUnityObjectList<in T> where T : Object
    {
        bool Remove(T item);
    }
}
