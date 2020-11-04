using Authentication.PlayFab;
using d4160.GameFramework.Authentication;
using NaughtyAttributes;
using PlayFab;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Menus
{
    public class LoginView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _emailField;
        [SerializeField] private TMP_InputField _passwordField;
        [SerializeField] private Button _loginButton;
        [SerializeField] private Button _getBackButton;

        void OnEnable()
        {
            _loginButton.interactable = true;
            _getBackButton.interactable = true;

            PlayFabAuthService.OnPlayFabError += PlayFabAuthServiceOnOnPlayFabError;
        }

        void OnDisable()
        {
            PlayFabAuthService.OnPlayFabError -= PlayFabAuthServiceOnOnPlayFabError;
        }

        private void PlayFabAuthServiceOnOnPlayFabError(PlayFabError error)
        {
            _loginButton.interactable = true;
            _getBackButton.interactable = true;
        }

        [Button]
        public void Login()
        {
            if (GameAuthSdk.HasSession)
            {
                Debug.LogError($"Already has a session: {GameAuthSdk.SessionTicket}");
                return;
            }

            if (string.IsNullOrEmpty(_emailField.text) || string.IsNullOrEmpty(_passwordField.text))
            {
                Debug.LogError("Email or password are null.");
                return;
            }

            _loginButton.interactable = false;
            _getBackButton.interactable = false;

            PlayFabAuthController.Instance.Authenticate(_emailField.text, _passwordField.text, PlayFabAuthTypes.EmailAndPassword);
        }
    }
}
