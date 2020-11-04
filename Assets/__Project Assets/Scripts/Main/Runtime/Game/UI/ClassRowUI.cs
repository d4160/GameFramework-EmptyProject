using d4160.Core.MonoBehaviours.ObjectCollections;
using Game.Menus;
using NaughtyAttributes;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ClassRowUI : UnityObjectItem<ClassRowUI>
    {
        [SerializeField] private TextMeshProUGUI _classText;
        [SerializeField] private TextMeshProUGUI _courseText;
        [SerializeField] private TextMeshProUGUI _currentMaxText;
        [SerializeField] private Button _joinButton;

        private RoomInfo _room;

        void OnEnable()
        {
            _joinButton.interactable = true;
        }

        [Button]
        public void JoinClass()
        {
            _joinButton.interactable = false;

            var success = PhotonNetwork.JoinRoom(_room.Name);

            if (!success)
            {
                _joinButton.interactable = true;
            }
        }

        public void UpdateUI(RoomInfo room)
        {
            _room = room;

            _classText.text = room.Name;
            _currentMaxText.text = $"{room.PlayerCount}/{room.MaxPlayers}";
            _courseText.text = room.CustomProperties[CreateClassView.COURSE_KEY].ToString();

            if (room.PlayerCount == room.MaxPlayers)
            {
                _joinButton.interactable = false;
            }
            else
            {
                _joinButton.interactable = true;
            }
        }
    }
}
