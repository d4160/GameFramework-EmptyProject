//using Authentication.PlayFab;
//using d4160.GameFramework.Authentication;
//using Game._Player;
//using NaughtyAttributes;
//using Persistence.PlayFab;
//using TMPro;
//using UltEvents;
//using UnityEngine;
//using UnityEngine.UI;

//namespace Game.Menus
//{
//    public class ProfileView : MonoBehaviour
//    {
//        [SerializeField] private TMP_InputField _displayNameField;
//        [SerializeField] private ToggleGroup _genreToggleGroup;
//        [SerializeField] private Toggle[] _genreToggles;
//        [SerializeField] private Button _saveButton;

//        [Header("EVENTS")]
//        [SerializeField] private UltEvent _onDisplayNameUpdateComplete;
//        [SerializeField] private UltEvent _onDisplayNameUpdateFail;

//        void OnEnable()
//        {
//            _saveButton.interactable = false;
//            _displayNameField.interactable = false;

//            _displayNameField.text = "Obteniendo nombre de usuario...";

//            PlayFabAuthController.Instance.GetDisplayName((s) =>
//            {
//                _displayNameField.text = s;
//                _saveButton.interactable = true;
//                _displayNameField.interactable = true;

//                SelectGenre();
//            });
//        }

//        [Button]
//        public void SavePlayerData()
//        {
//            if (!GameAuthSdk.HasSession)
//            {
//                Debug.LogError($"You need to start a session before update your name.");
//                return;
//            }

//            if (string.IsNullOrEmpty(_displayNameField.text))
//            {
//                return;
//            }

//            _saveButton.interactable = false;
//            _displayNameField.interactable = false;

//            PlayFabAuthController.Instance.UpdateDisplayName(_displayNameField.text, () =>
//            {
//                _onDisplayNameUpdateComplete?.Invoke();

//                SaveGenre();
//            }, () =>
//            {
//                _saveButton.interactable = true;
//                _displayNameField.interactable = true;

//                _onDisplayNameUpdateFail?.Invoke();
//            });
//        }

//        private void SaveGenre()
//        {
//            PlayerManager.Instance.LocalPlayerGenre = SelectedGenre;

//            PlayFabPersistenceController.Instance.Save();
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

//        private void SelectGenre()
//        {
//            if (!string.IsNullOrEmpty(PlayerManager.Instance.LocalPlayerGenre))
//            {
//                _genreToggleGroup.SetAllTogglesOff(false);

//                string genre = PlayerManager.Instance.LocalPlayerGenre;
//                for (var i = 0; i < _genreToggles.Length; i++)
//                {
//                    //Debug.Log($"{_genreToggles[i].name} {PlayerManager.Instance.LocalPlayerGenre}");
//                    if (_genreToggles[i].name != genre) continue;
//                    _genreToggles[i].isOn = true;
//                    break;
//                }
//            }
//            else
//            {
//                Debug.LogError("An item for player is missing, load one first", gameObject);
//            }
//        }
//    }
//}
