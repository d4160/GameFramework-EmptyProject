using UnityEngine;

namespace d4160.GameFramework.DataPersistence
{
    public abstract class BaseTitleDataScriptable<T> : BaseTitleDataScriptable where T : ScriptableObject
    {
        public static T Instance => TitleDataSettings.Instance.TitleDatabaseScriptable as T;
    }

    public abstract class BaseTitleDataScriptable : ScriptableObject, ITitleDatabase
    {
        protected IUnityDataSerializer _serializer;

        public void SetSerializer(IUnityDataSerializer serializer)
        {
            _serializer = serializer;
        }

        public virtual string Serialize()
        {
            return _serializer?.Serialize(this);
        }

        public virtual void Deserialize(string data)
        {
            if(!string.IsNullOrEmpty(data))
                _serializer?.Deserialize(data, this);
        }
    }
}
