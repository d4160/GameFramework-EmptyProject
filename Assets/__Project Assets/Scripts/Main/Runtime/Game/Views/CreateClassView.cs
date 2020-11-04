using Authentication.PlayFab;
using Game.UI;
using NaughtyAttributes;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Menus
{
    public class CreateClassView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _className;
        [SerializeField] private TMP_InputField _courseName;
        [SerializeField] private Slider _maxCapacity;
        [SerializeField] private Button _createClassButton;
        [SerializeField] private Button _backButton;

        public const string COURSE_KEY = "c";

        void OnEnable()
        {
            _createClassButton.interactable = true;
            _backButton.interactable = true;
        }

        [Button]
        public void CreateClass()
        {
            _createClassButton.interactable = false;
            _backButton.interactable = false;

            if (string.IsNullOrEmpty(_className.text) ||
                string.IsNullOrEmpty(_courseName.text))
            {
                return;
            }

            var customRoomPropertiesForLobby = new string[1];
            customRoomPropertiesForLobby[0] = COURSE_KEY;

            var customRoomProperties = new ExitGames.Client.Photon.Hashtable { {COURSE_KEY, _courseName.text} };

            var success = PhotonNetwork.CreateRoom(
                _className.text, new RoomOptions { MaxPlayers = (byte)_maxCapacity.value, CustomRoomPropertiesForLobby = customRoomPropertiesForLobby, CustomRoomProperties = customRoomProperties }, TypedLobby.Default);

            if (!success)
            {
                _createClassButton.interactable = true;
                _backButton.interactable = true;
            }
        }

        public void SetInteractableButtons(bool interactable)
        {
            _createClassButton.interactable = interactable;
            _backButton.interactable = interactable;
        }

    }
}
