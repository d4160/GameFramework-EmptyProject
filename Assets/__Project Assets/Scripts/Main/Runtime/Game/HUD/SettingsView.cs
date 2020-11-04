//using ExitGames.Client.Photon;
//using Game._Player;
//using Photon.Pun;
//using Photon.Realtime;
//using UnityEngine;
//using UnityEngine.UI;

//namespace Game.Menus
//{
//    public class SettingsView : MonoBehaviour, IInRoomCallbacks
//    {
//        [SerializeField] private GameObject _basicSettingsPanel;
//        [SerializeField] private Toggle _basicSettingsToggle;
//        [SerializeField] private Slider _qualitySlider;
//        [SerializeField] private Toggle _muteToggle;

//        void OnEnable()
//        {
//            PhotonNetwork.AddCallbackTarget(this);
//        }

//        void OnDisable()
//        {
//            PhotonNetwork.RemoveCallbackTarget(this);
//        }

//        public void ToggleMute(bool value)
//        {
//            PlayerManager.Instance.Recorder.TransmitEnabled = !value;
//;
//            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable() { { PlayerController.PlayerIsMuted_Key, value } });
//        }

//        public void ChangeQuality(float value)
//        {
//            QualitySettings.SetQualityLevel((int)value);
//        }

//        public void OnPlayerEnteredRoom(Player newPlayer)
//        {
//        }

//        public void OnPlayerLeftRoom(Player otherPlayer)
//        {
//        }

//        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
//        {
//        }

//        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
//        {
//            if (PhotonNetwork.LocalPlayer == targetPlayer)
//            {
//                var muteValue = (bool) changedProps[PlayerController.PlayerIsMuted_Key];
//                _muteToggle.SetIsOnWithoutNotify(muteValue);

//                PlayerManager.Instance.Recorder.TransmitEnabled = !muteValue;
//            }
//        }

//        public void OnMasterClientSwitched(Player newMasterClient)
//        {
            
//        }
//    }
//}
