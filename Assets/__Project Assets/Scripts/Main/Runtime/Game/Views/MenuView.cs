//using d4160.GameFramework.Authentication;
//using Data.Scriptables;
//using Game._Player;
//using Game.UI;
//using NaughtyAttributes;
//using Photon.Pun;
//using UnityEngine;
//using UnityEngine.GameFoundation;
//using UnityEngine.UI;

//namespace Game.Menus
//{
//    public class MenuView : MonoBehaviour
//    {
//        [SerializeField] private Button _logoutButton;
//        [SerializeField] private Button _refreshButton;
//        [SerializeField] private MenuOptionUIManager _menuOptions;

//        void OnEnable()
//        {
//            _refreshButton.interactable = true;
//            _logoutButton.interactable = true;

//            SyncPermissions();
//        }

//        [Button]
//        public void SyncPermissions()
//        {
//            if (PlayerManager.Instance.LocalPlayerRole != null)
//            {
//                int role = PlayerManager.Instance.LocalPlayerRole.Value;

//                var instance = TitleDatabaseScriptable.Instance;
//                int[] permissions = instance.GetPermissions(role, 1);

//                //permissions.DebugLog();

//                for (var i = 0; i < _menuOptions.Count; i++)
//                {
//                    _menuOptions[i].gameObject.SetActive(false);
//                    for (var j = 0; j < permissions.Length; j++)
//                    {
//                        if (i + 1 == permissions[j])
//                        {
//                            _menuOptions[i].gameObject.SetActive(true);
//                            break;
//                        }
//                    }
//                }
//            }
//            else
//            {
//                Debug.LogError("An item for player is missing, load one first", gameObject);
//            }
//        }

//        [Button]
//        public void Logout()
//        {
//            if (!GameAuthSdk.HasSession)
//            {
//                Debug.LogError($"You need to start a session before update your name.", gameObject);
//                return;
//            }

//            _refreshButton.interactable = false;
//            _logoutButton.interactable = false;

//            PlayerManager.Instance.ResetData();
//            GameAuthSdk.Unauthenticate();
//            GameFoundationSdk.Uninitialize();
//            PhotonNetwork.Disconnect();
//        }
//    }
//}