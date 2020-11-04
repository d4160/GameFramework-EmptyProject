using System.Collections.Generic;
using Game.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Menus
{
    public class JoinClassView : MonoBehaviour, ILobbyCallbacks
    {
        [SerializeField] private ClassTableUI _classTable;
        [SerializeField] private Button _backButton;

        void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void OnJoinedLobby()
        {
        }

        public void OnLeftLobby()
        {
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            _classTable.ResetUpdateIndex();
            _classTable.DisableOtherFromUpdateIndex();

            for (int i = 0; i < roomList.Count; i++)
            {
                if(roomList[i] == null || roomList[i].PlayerCount == 0) continue;

                ClassRowUI classRow = _classTable.AddOrUpdateObject();
                classRow.UpdateUI(roomList[i]);
            }
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            
        }
    }
}
