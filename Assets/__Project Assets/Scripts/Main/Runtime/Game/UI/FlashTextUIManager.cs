using d4160.Core.MonoBehaviours;
using NaughtyAttributes;
using UnityEngine;

namespace Game.UI
{
    public class FlashTextUIManager : UnityObjectListSingleton<FlashTextUIManager, FlashTextUI>
    {
        public override FlashTextUI AddOrUpdateObject(Vector3 position, Quaternion rotation, Transform parent = null,
            bool worldPositionStays = true)
        {
            var flashText = base.AddOrUpdateObject(position, rotation, parent, worldPositionStays);

            flashText.Manager = this;

            return flashText;
        }

        public override FlashTextUI AddObject(Vector3 position, Quaternion rotation, Transform parent = null, bool worldPositionStays = true)
        {
            var flashText = base.AddObject(position, rotation, parent, worldPositionStays);
            flashText.Manager = this;

            return flashText;
        }

        [Button]
        public void AddObjectTest()
        {
            AddObject();
        }
    }
}
