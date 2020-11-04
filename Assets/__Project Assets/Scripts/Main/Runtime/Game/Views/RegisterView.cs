//using System.Collections.Generic;
//using Authentication.PlayFab;
//using d4160.GameFramework.Authentication;
//using Game._Player;
//using NaughtyAttributes;
//using Persistence.PlayFab;
//using PlayFab;
//using PlayFab.ClientModels;
//using TMPro;
//using UnityEngine;
//using UnityEngine.GameFoundation;
//using UnityEngine.UI;

//namespace Game.Menus
//{
//    public class RegisterView : MonoBehaviour
//    {
//        [SerializeField] private TMP_InputField _emailField;
//        [SerializeField] private TMP_InputField _passwordField;
//        [SerializeField] private ToggleGroup _genreToggleGroup;
//        [SerializeField] private Toggle[] _genreToggles;
//        [SerializeField] private Button _registerButton;
//        [SerializeField] private Button _getBackButton;

//        void OnEnable()
//        {
//            _registerButton.interactable = true;
//            _getBackButton.interactable = true;

//            PlayFabAuthService.OnPlayFabError += PlayFabAuthServiceOnOnPlayFabError;
//        }

//        void OnDisable()
//        {
//            PlayFabAuthService.OnPlayFabError -= PlayFabAuthServiceOnOnPlayFabError;
//        }

//        private void PlayFabAuthServiceOnOnPlayFabError(PlayFabError error)
//        {
//            _registerButton.interactable = true;
//            _getBackButton.interactable = true;
//        }

//        [Button]
//        public void Register()
//        {
//            if (string.IsNullOrEmpty(_emailField.text) || string.IsNullOrEmpty(_passwordField.text))
//            {
//                Debug.LogError("Email or password is null.");
//                return;
//            }

//            _registerButton.interactable = false;
//            _getBackButton.interactable = false;

//            PlayFabAuthController.Instance.Authenticate(_emailField.text, _passwordField.text, PlayFabAuthTypes.RegisterPlayFabAccount);
//        }

//        [Button]
//        public void SetPlayerData()
//        {
//            if(!gameObject.activeSelf)
//                return;

//            PlayerManager.Instance.LocalPlayerGenre = SelectedGenre;
//        }

//        private string SelectedGenre
//        {
//            get
//            {
//                for (int i = 0; i < _genreToggles.Length; i++)
//                {
//                    if (_genreToggles[i].isOn)
//                    {
//                        return _genreToggles[i].name;
//                    }
//                }

//                return "N";
//            }
//        }
//    }
//}
