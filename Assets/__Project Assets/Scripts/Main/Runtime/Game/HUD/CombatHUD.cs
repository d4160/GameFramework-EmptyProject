using UltEvents;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.UI
{
    public class CombatHUD : MonoBehaviour
    {
        [SerializeField] private float _countDownTime;

        [SerializeField] private Text[] _playerNames;
        [SerializeField] private Text _countDownText;

        [Header("EVENTS")] [SerializeField] private UltEvent _onCountDownBecomeZero;

        void Update()
        {
            if (_countDownTime > 0)
            {
                _countDownTime -= Time.deltaTime;

                if (_countDownTime < 0)
                    _countDownTime = 0;

                _countDownText.text = ((int)_countDownTime).ToString();

                if (_countDownTime == 0)
                {
                    OnCountDownBecameZero();
                }
            }
        }

        private void OnCountDownBecameZero()
        {
            SceneManager.LoadScene("Main Menu - Test", LoadSceneMode.Single);

            _onCountDownBecomeZero?.Invoke();
        }

        public void SetPlayerName(string playerName, int idx)
        {
            _playerNames[idx].text = playerName;
        }
    }
}
