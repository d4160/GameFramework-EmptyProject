using System;
using System.Collections;
using d4160.Core.MonoBehaviours;
using d4160.GameFramework.DataPersistence;
using NaughtyAttributes;
using UltEvents;
using UnityEngine;
using UnityEngine.GameFoundation;
using UnityEngine.GameFoundation.DefaultLayers;
using UnityEngine.Promise;

namespace Persistence.PlayFab
{
    public class PlayFabPersistenceController : Singleton<PlayFabPersistenceController>
    {
        [SerializeField] private string _key;
        [Header("EVENTS")] 
        [SerializeField] private UltEvent _onInitOrLoadComplete;
        [SerializeField] private UltEvent _onInitOrLoadFail;

        private PersistenceDataLayer _dataLayer;

        public event Action OnInitOrLoadComplete;

        [Button]
        public void Initialize()
        {
            var serializer = new JsonDataSerializer();
            var persistence = new PlayFabPersistence(_key, serializer);
            _dataLayer = new PersistenceDataLayer(persistence);

            Deferred def = GameFoundationSdk.Initialize(_dataLayer);
            if (def.isDone)
            {
                if (def.isFulfilled)
                {
                    _onInitOrLoadComplete?.Invoke();
                    OnInitOrLoadComplete?.Invoke();
                }
                else
                {
                    Debug.LogError(def.error.Message);
                    _onInitOrLoadFail?.Invoke();
                }
            }
            else
            {
                IEnumerator Routine(Deferred aDef)
                {
                    yield return aDef.Wait();

                    if (aDef.isFulfilled)
                    {
                        _onInitOrLoadComplete?.Invoke();
                        OnInitOrLoadComplete?.Invoke();
                    }
                    else
                    {
                        Debug.LogError(aDef.error.Message);
                        _onInitOrLoadFail?.Invoke();
                    }
                }

                StartCoroutine(Routine(def));
            }
        }

        [Button]
        public void Save()
        {
            Save(null);
        }


        public void Save(Action onComplete)
        {
            var saveOperation = _dataLayer.Save();

            if (saveOperation.isDone)
            {
                LogSaveOperationCompletion(saveOperation);
            }
            else
            {
                StartCoroutine(WaitForSaveCompletion(saveOperation, onComplete));
            }
        }

        private IEnumerator WaitForSaveCompletion(Deferred saveOperation, Action onComplete)
        {
            yield return saveOperation.Wait();

            onComplete?.Invoke();

            LogSaveOperationCompletion(saveOperation);
        }

        private void LogSaveOperationCompletion(Deferred saveOperation)
        {
            if (saveOperation.isFulfilled)
            {
                Debug.Log("Saved!");
            }
            else
            {
                Debug.LogError($"Save failed! Error: {saveOperation.error}");
            }
        }

        [Button]
        public void Load()
        {
            GameFoundationSdk.Uninitialize();

            if (_dataLayer?.persistence == null)
            {
                Debug.LogError("DataLayer is null.");
                return;
            }

            Deferred def = GameFoundationSdk.Initialize(_dataLayer);
            if (def.isDone)
            {
                if (def.isFulfilled)
                {
                    _onInitOrLoadComplete?.Invoke();
                    OnInitOrLoadComplete?.Invoke();
                }
                else
                {
                    Debug.LogError(def.error.Message);
                    _onInitOrLoadFail?.Invoke();
                }
            }
            else
            {
                IEnumerator Routine(Deferred aDef)
                {
                    yield return aDef.Wait();

                    if (aDef.isFulfilled)
                    {
                        _onInitOrLoadComplete?.Invoke();
                        OnInitOrLoadComplete?.Invoke();
                    }
                    else
                    {
                        Debug.LogError(aDef.error.Message);
                        _onInitOrLoadFail?.Invoke();
                    }
                }

                StartCoroutine(Routine(def));
            }
        }
    }
}
