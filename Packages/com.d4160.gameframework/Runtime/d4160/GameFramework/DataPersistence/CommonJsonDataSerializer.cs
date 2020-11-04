using UnityEngine;

namespace d4160.GameFramework.DataPersistence
{
    /// <summary>
    /// DataSerializer to serialize GameFoundation's data to and from Json.
    /// </summary>
    public sealed class CommonJsonDataSerializer : ICommonDataSerializer
    {
        /// <inheritdoc />
        public string Serialize(object data)
        {
            var json = JsonUtility.ToJson(data);
            return json;
        }

        /// <inheritdoc />
        public T Deserialize<T>(string serializedData)
        {
            return JsonUtility.FromJson<T>(serializedData);
        }
    }
}
