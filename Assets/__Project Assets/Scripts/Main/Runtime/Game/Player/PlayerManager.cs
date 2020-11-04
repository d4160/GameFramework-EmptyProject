//using System.Collections.Generic;
//using d4160.Core;
//using d4160.Core.MonoBehaviours;
//using Game.World;
//using NaughtyAttributes;
//using Photon.Pun;
////using Photon.Voice.Unity;
//using UltEvents;
//using UnityEngine;
//using UnityEngine.GameFoundation;

//namespace Game._Player
//{
//    public class PlayerManager : UnityObjectListSingleton<PlayerManager, GameObject>
//    {
//        [SerializeField] private Recorder _recorder;

//        [Header("SPAWN")]
//        [SerializeField] private Transform[] _studentSpawnPoints;
//        [SerializeField] private Transform[] _teacherSpawnPoints;

//        [Header("NETWORKING")]
//        [SerializeField] private readonly List<PlayerSync> _remotePlayers = new List<PlayerSync>();

//        [Header("EVENTS")] [SerializeField] private UltEvent _onPlayerDataLoadComplete;

//        private InventoryItem _playerItem;

//        private GameObject _localPlayer;
//        private PhotonView _localPhotonView;

//        public GameObject LocalPlayer => _localPlayer;
//        public PhotonView LocalPhotonView => _localPhotonView;

//        public string LocalPlayerGenre
//        {
//            get => _playerItem?.GetMutableProperty("Genre");
//            set
//            {
//                if (value != null) _playerItem?.SetMutableProperty("Genre", value);
//            }
//        }

//        public int? LocalPlayerRole 
//        { 
//            get =>_playerItem?.GetMutableProperty("Role");
//            set
//            {
//                if (value != null) _playerItem?.SetMutableProperty("Role", value.Value);
//            }
//        }

//        public Recorder Recorder => _recorder;

//        [Button]
//        public void LoadPlayerData()
//        {
//            Tag playerTag = GameFoundationSdk.tags.Find("Player");
//            List<InventoryItem> playerItems = new List<InventoryItem>();
//            GameFoundationSdk.inventory.FindItems(playerTag, playerItems);

//            if (playerItems.Count > 0)
//                _playerItem = playerItems[0];

//            if (_playerItem == null)
//            {
//                GameParameter playerParameter = GameFoundationSdk.catalog.Find<GameParameter>("Player");
//                Property genreParam = playerParameter["Genre"];
//                Property roleParam = playerParameter["Role"];

//                InventoryItemDefinition playerItemDef =
//                    GameFoundationSdk.catalog.Find<InventoryItemDefinition>("player");
//                _playerItem = GameFoundationSdk.inventory.CreateItem(playerItemDef);
//                _playerItem.SetMutableProperty("Genre", genreParam);
//                _playerItem.SetMutableProperty("Role", roleParam);

//                Debug.Log("Added new Player Item.", gameObject);
//            }
//            else
//            {
//                Debug.Log("Player Item loaded.", gameObject);
//            }

//            _onPlayerDataLoadComplete?.Invoke();
//        }

//        [Button]
//        public void SetDefaultPlayerData()
//        {
//            GameParameter playerParameter = GameFoundationSdk.catalog.Find<GameParameter>("Player");
//            Property genreParam = playerParameter["Genre"];
//            Property roleParam = playerParameter["Role"];

//            SetPlayerData(roleParam, genreParam.ToString()[0]);
//        }

//        public void SetPlayerData(int role, char genre)
//        {
//            if (_playerItem == null)
//            {
//                Debug.LogError("An item for player is missing, load one first", gameObject);
//                return;
//            }

//            _playerItem.SetMutableProperty("Genre", genre);
//            _playerItem.SetMutableProperty("Role", role);
//        }

//        [Button]
//        public void InstanceCharacter()
//        {
//            var provider = GameObjectProvider as PlayerProvider;

//            if (LocalPlayerRole.HasValue && !string.IsNullOrEmpty(LocalPlayerGenre) && provider != null)
//            {
//                provider.SelectPool(LocalPlayerRole.Value, LocalPlayerGenre);

//                Transform randomLocation = LocalPlayerRole == 1 ? _studentSpawnPoints.Random() : _teacherSpawnPoints.Random();
//                _localPlayer = AddObject(randomLocation.position, randomLocation.rotation);

//                CampusManager.Instance.SetActiveClassRoom(true);
//            }
//        }

//        [Button]
//        public void DestroyCharacter()
//        {
//            Remove(_localPlayer);
//        }

//        public void AddRemotePlayer(PlayerSync remotePlayer)
//        {
//            if (!_remotePlayers.Contains(remotePlayer))
//            {
//                _remotePlayers.Add(remotePlayer);
//            }
//        }

//        public void RemoveRemotePlayer(PlayerSync remotePlayer)
//        {
//            _remotePlayers.Remove(remotePlayer);
//        }

//        public void RegisterLocalPhotonView(PlayerSync localSync)
//        {
//            _localPhotonView = localSync.photonView;
//        }

//        [Button]
//        public void ResetData()
//        {
//            _playerItem = null;
//        }

        
//    }
//}
