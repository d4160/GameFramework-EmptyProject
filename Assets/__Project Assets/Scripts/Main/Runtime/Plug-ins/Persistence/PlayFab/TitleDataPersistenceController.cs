using System.Collections.Generic;
using d4160.GameFramework.DataPersistence;
using Data.Scriptables;
using NaughtyAttributes;
using PlayFab;
//using PlayFab.ServerModels;
using UnityEngine;
using UnityEngine.GameFoundation;

namespace Persistence.PlayFab
{
    public class TitleDataPersistenceController : MonoBehaviour
    {
        [SerializeField] private string _key;

        private TitleDatabaseScriptable _titleDatabase;

        void Start()
        {
            _titleDatabase = TitleDatabaseScriptable.Instance;
            _titleDatabase.SetSerializer(new UnityJsonDataSerializer());

            Load();
        }

        [Button]
        public void Save()
        {
            var json = _titleDatabase.Serialize();

            //PlayFabServerAPI.SetTitleData(new SetTitleDataRequest()
            //{
            //    Key = _key,
            //    Value = json
            //}, (result) =>
            //{
            //    Debug.Log("Title data saved.");
            //}, (error) =>
            //{
            //    Debug.Log("Title data save error.");
            //});
        }

        [Button]
        public void Load()
        {
            //PlayFabServerAPI.GetTitleData(new GetTitleDataRequest()
            //{
            //    Keys = new List<string>{ _key }
            //}, (result) =>
            //{
            //    _titleDatabase.Deserialize(result.Data[_key]);
            //    Debug.Log("Title data loaded.");
            //}, (error) =>
            //{
            //    Debug.Log("Title data load error.");
            //});
        }
    }
}
