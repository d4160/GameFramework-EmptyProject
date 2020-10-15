using d4160.GameFramework.Authentication;
using NaughtyAttributes;
using UnityEngine;

namespace Authentication.Photon2
{
    public class Photon2AuthController : MonoBehaviour
    {
        [SerializeField] private string _displayName;

        private PhotonAuthService _authService = PhotonAuthService.Instance;

        [Button]
        public void UpdateDisplayName()
        {
            _authService.DisplayName = _displayName;

            Debug.Log($"DisplayName updated to: {_displayName}");
        }
    }
}
