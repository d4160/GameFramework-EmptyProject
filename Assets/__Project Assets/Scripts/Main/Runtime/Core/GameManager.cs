using d4160.Core.MonoBehaviours;
using Game._Camera;
using Game._Player;
using NaughtyAttributes;
using Photon.Pun;
//using Runtime.Game.Menus;
using UnityEngine;

namespace Core
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private GameState _gameState;

        public GameState GameState => _gameState;

        void Update()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                // Check if Back was pressed this frame
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ReturnApp();
                }
            }
            else if (Input.GetKeyDown(KeyCode.F4))
            {
                ReturnApp();
            }

        }

        [Button]
        private void ReturnApp()
        {
            switch (_gameState)
            {
                case GameState.InLobby:
                    if (Application.isPlaying)
                    {
                        Application.Quit();

#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#endif
                    }

                    break;
                case GameState.InClass:
                    PhotonNetwork.LeaveRoom();
                    break;
            }
        }

        public void SetState(GameState newState)
        {
            _gameState = newState;
        }

        public void AfterLeaveRoom()
        {
            //PlayerManager.Instance.DestroyCharacter();

            CameraManager.Instance.SetActiveFirstPerson(true);
            //ViewManager.Instance.CreateClassView.SetInteractableButtons(true);
            //ViewManager.Instance.UserListView.ClearTable();

            _gameState = GameState.InLobby;
        }
    }

    public enum GameState
    {
        InLobby,
        InClass
    }
}