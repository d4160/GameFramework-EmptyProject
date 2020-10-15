using d4160.Core.MonoBehaviours;
using d4160.GameFramework.DataPersistence;
using NaughtyAttributes;
using UltEvents;
using UnityEngine;
using UnityEngine.GameFoundation;
using UnityEngine.GameFoundation.DefaultLayers;

namespace Persistence.PlayFab
{
    public class PlayFabPersistenceController : Singleton<PlayFabPersistenceController>
    {
        [SerializeField] private string _key;
        [Header("EVENTS")] [SerializeField] private UltEvent _onLoaded;

        private PersistenceDataLayer _dataLayer;

        public UltEvent OnLoaded => _onLoaded;

        [Button]
        public void Initialize()
        {
            var serializer = new JsonDataSerializer();
            var persistence = new PlayFabPersistence(_key, serializer);
            _dataLayer = new PersistenceDataLayer(persistence);

            GameFoundation.Initialize(_dataLayer, () =>
            {
                _onLoaded?.Invoke();
            });
        }

        [Button]
        public void Save()
        {
            _dataLayer.Save();
        }

        [Button]
        public void Load()
        {
            GameFoundation.Uninitialize();

            GameFoundation.Initialize(_dataLayer);
        }
    }
}
