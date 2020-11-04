//using UltEvents;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//namespace Game.Menus
//{
//    public class VSMenu : MonoBehaviour
//    {
//        [SerializeField] private float _countDownTime;

//        [SerializeField] private Text[] _playerNames;
//        [SerializeField] private Text _countDownText;

//        [Header("EVENTS")] [SerializeField] private UltEvent _onCountDownBecomeZero;

//        void Update()
//        {
//            if (_countDownTime > 0)
//            {
//                _countDownTime -= Time.deltaTime;

//                if (_countDownTime < 0)
//                    _countDownTime = 0;

//                _countDownText.text = ((int)_countDownTime).ToString();

//                if (_countDownTime == 0)
//                {
//                    OnCountDownBecameZero();
//                }
//            }
//        }

//        private void OnCountDownBecameZero()
//        {
//            SceneManager.LoadScene("Level UI - Test", LoadSceneMode.Single);

//            _onCountDownBecomeZero?.Invoke();
//        }

//        public void SetPlayerName(string playerName, int idx)
//        {
//            _playerNames[idx].text = playerName;
//        }

//        public void LoadPlayerSelectionForPlayer(int playerIdx)
//        {
//            PlayerSelectionMenu.Player = playerIdx;

//            SceneManager.LoadScene("Player Selection - Test", LoadSceneMode.Single);
//        }
//    }
//}
