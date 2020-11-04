//using d4160.Core;
//using d4160.Core.MonoBehaviours.ObjectCollections;
//using ExitGames.Client.Photon;
//using Game._Player;
//using NaughtyAttributes;
//using Photon.Realtime;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//namespace Game.UI
//{
//    public class UserRowUI : UnityObjectItem<ClassRowUI>, IObjectData<Player>
//    {
//        [SerializeField] private Image _portrait;
//        [SerializeField] private Image _background;
//        [SerializeField] private TextMeshProUGUI _genreText;
//        [SerializeField] private TextMeshProUGUI _displayNameText;
//        [SerializeField] private Toggle _muteToggle;

//        private Player _player;

//        public void SwitchMute(bool value)
//        {
//            _player.SetCustomProperties(new Hashtable() { { PlayerController.PlayerIsMuted_Key, value } });
//        }

//        public bool IsPlayer(Player player)
//        {
//            return player == _player;
//        }

//        public bool IsPlayerNull()
//        {
//            return _player == null;
//        }

//        public Player GetData()
//        {
//            return _player;
//        }

//        public void SetData(Player player)
//        {
//            _player = player;

//            _displayNameText.text = player.NickName;
//            _genreText.text = player.CustomProperties[PlayerController.PlayerGenre_Key].ToString();
//            _muteToggle.SetIsOnWithoutNotify((bool)player.CustomProperties[PlayerController.PlayerIsMuted_Key]);
//        }

//        public void SetBackgroundColor(Color color)
//        {
//            _background.color = color;
//        }

//        public void SetActiveMuteToggle(bool active)
//        {
//            _muteToggle.gameObject.SetActive(active);
//        }

//        public void SetMuteToggleState(bool isOn)
//        {
//            _muteToggle.SetIsOnWithoutNotify(isOn);
//        }
//    }
//}