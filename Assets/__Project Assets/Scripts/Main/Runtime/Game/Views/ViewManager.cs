//using System.Linq;
//using d4160.Core.MonoBehaviours;
//using Data.Scriptables;
//using Game._Player;
//using Game.Menus;
//using NaughtyAttributes;
//using UnityEngine;

//namespace Runtime.Game.Menus
//{
//    public class ViewManager : Singleton<ViewManager>
//    {
//        [SerializeField] private GameObject _teacherCanvas;
//        [SerializeField] private CreateClassView _createClassView;
//        [SerializeField] private UserListView _userListView;

//        public CreateClassView CreateClassView => _createClassView;
//        public UserListView UserListView => _userListView;

//        void Start()
//        {
//            _teacherCanvas.SetActive(false);
//        }

//        [Button]
//        public void SyncPermissions()
//        {
//            if (PlayerManager.Instance.LocalPlayerRole == null) return;
            
//            var permissions =
//                TitleDatabaseScriptable.Instance.GetPermissions(PlayerManager.Instance.LocalPlayerRole.Value, 2);

//            _teacherCanvas.SetActive(permissions.Contains(3));
//        }
//    }
//}
