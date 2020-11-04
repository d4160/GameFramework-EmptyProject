using UnityEngine;

namespace d4160.GameFramework.DataPersistence
{
    /// <summary>
    /// DataSerializer to serialize GameFoundation's data to and from Json.
    /// </summary>
    public sealed class UnityJsonDataSerializer : IUnityDataSerializer
    {
        /// <inheritdoc />
        public string Serialize(Object data)
        {
            var json = JsonUtility.ToJson(data);
            return json;
        }

        /// <inheritdoc />
        public void Deserialize(string serializedData, Object objectToOverride)
        {
            JsonUtility.FromJsonOverwrite(serializedData, objectToOverride);
        }
    }
}
