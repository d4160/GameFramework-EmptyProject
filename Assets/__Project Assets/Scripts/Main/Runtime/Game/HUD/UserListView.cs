//using System.Collections.Generic;
//using Data.Scriptables;
//using ExitGames.Client.Photon;
//using Game._Player;
//using Game.UI;
//using NaughtyAttributes;
//using Photon.Pun;
//using Photon.Realtime;
//using UnityEngine;

//namespace Game.Menus
//{
//    public class UserListView : MonoBehaviour, IInRoomCallbacks
//    {
//        [SerializeField] private UserTableUI _userTable;
//        [SerializeField] private Color _meColor;
//        [SerializeField] private Color _otherColor;

//        private bool _canMuteParticipants;
//        private bool _started;

//        void OnEnable()
//        {
//            PhotonNetwork.AddCallbackTarget(this);

//            SyncPermissions();

//            if (!_started)
//                return;

//            if (PhotonNetwork.CurrentRoom != null)
//                UpdatePlayerList(PhotonNetwork.CurrentRoom.Players);
//        }

//        void Start()
//        {
//            if (PhotonNetwork.CurrentRoom != null)
//                UpdatePlayerList(PhotonNetwork.CurrentRoom.Players);

//            _started = true;
//        }

//        void OnDisable()
//        {
//            PhotonNetwork.RemoveCallbackTarget(this);
//        }

//        public void UpdatePlayerList(Dictionary<int, Player> playerList)
//        {
//            _userTable.ResetUpdateIndex();
//            _userTable.DisableOtherFromUpdateIndex();

//            foreach (var player in playerList)
//            {
//                if (player.Value == null)
//                    continue;

//                UserRowUI classRow = _userTable.AddOrUpdateObject();
//                classRow.SetData(player.Value);
//                classRow.SetActiveMuteToggle(_canMuteParticipants);

//                var isLocalPlayer = PhotonNetwork.LocalPlayer == player.Value;
//                classRow.SetBackgroundColor(isLocalPlayer ? _meColor : _otherColor);
//            }
//        }

//        public void OnPlayerEnteredRoom(Player newPlayer)
//        {
//            if (newPlayer == null)
//                return;

//            UserRowUI classRow = _userTable.AddOrUpdateObject();

//            classRow.SetData(newPlayer);
//            classRow.SetActiveMuteToggle(_canMuteParticipants);

//            var isLocalPlayer = PhotonNetwork.LocalPlayer == newPlayer;
//            classRow.SetBackgroundColor(isLocalPlayer ? _meColor : _otherColor);
//        }

//        public void OnPlayerLeftRoom(Player otherPlayer)
//        {
//            _userTable.Remove(otherPlayer);
//        }

//        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
//        {
//        }

//        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
//        {
//            _userTable.SetMuteToggleState(targetPlayer, (bool)changedProps[PlayerController.PlayerIsMuted_Key]);
//        }

//        public void OnMasterClientSwitched(Player newMasterClient)
//        {
//        }

//        [Button]
//        public void SyncPermissions()
//        {
//            if (PlayerManager.Instance.LocalPlayerRole != null)
//            {
//                var permissions =
//                    TitleDatabaseScriptable.Instance.GetPermissions(PlayerManager.Instance.LocalPlayerRole.Value, 2);

//                _canMuteParticipants = permissions.Contains(4);
//            }
//        }

//        [Button]
//        public void ClearTable()
//        {
//            _userTable.Clear();
//        }
//    }
//}