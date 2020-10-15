using d4160.GameFramework.Authentication;
using NaughtyAttributes;
using UltEvents;
using UnityEngine;

namespace Authentication.PlayFab
{
    public class PlayFabAuthController : MonoBehaviour
    {
        [SerializeField] private string _displayName;

        [Header("EVENTS")] [SerializeField] private UltEvent _onAuthenticated; 

        private PlayFabAuthService _authService = PlayFabAuthService.Instance;

        void Start()
        {
            Authenticate();
        }

        [Button]
        public void Authenticate()
        {
            _authService.AuthType = PlayFabAuthTypes.Silent;
            _authService.DisplayName = _displayName;
            _authService.AuthenticateToPhotonAfterLogin = true;

            GameAuth.Authenticate(_authService, () =>
            {
                UpdateDisplayName();

                _onAuthenticated?.Invoke();
            });
        }

        [Button]
        public void Unauthenticate()
        {
            GameAuth.Unauthenticate();
        }

        [Button]
        public void UpdateDisplayName()
        {
            _authService.DisplayName = _displayName;

            _authService.UpdateDisplayName(
                (r) =>
                {
                    Debug.Log($"DisplayName updated to: {r.DisplayName}");
                }, (e) =>
                {
                    Debug.LogError(e.ErrorMessage);
                });
        }
    }
}
