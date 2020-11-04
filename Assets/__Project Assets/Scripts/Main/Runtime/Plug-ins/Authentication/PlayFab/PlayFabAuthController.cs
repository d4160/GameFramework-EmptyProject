using System;
using System.Collections;
using d4160.Core.MonoBehaviours;
using d4160.GameFramework.Authentication;
using NaughtyAttributes;
using UltEvents;
using UnityEngine;
using UnityEngine.Promise;

namespace Authentication.PlayFab
{
    public class PlayFabAuthController : Singleton<PlayFabAuthController>
    {
        [SerializeField] private string _displayName;
        [SerializeField] private string _email;
        [SerializeField] private string _password;

        [Header("EVENTS")] 
        [SerializeField] private UltEvent _onAuthComplete;
        [SerializeField] private UltEvent _onAuthFail;

        private readonly PlayFabAuthService _authService = PlayFabAuthService.Instance;

        [Button]
        public void Login()
        {
            Authenticate(_email, _password, PlayFabAuthTypes.EmailAndPassword);
        }

        [Button]
        public void Register()
        {
            Authenticate(_email, _password, PlayFabAuthTypes.RegisterPlayFabAccount);
        }

        public void Authenticate(string email, string password, PlayFabAuthTypes type)
        {
            _authService.SetDisplayName(null);
            _authService.AuthType = type;
            _authService.Email = email;
            _authService.Password = password;
            _authService.AuthenticateToPhotonAfterLogin = true;

            Deferred def = GameAuthSdk.Authenticate(_authService);

            if (def.isDone)
            {
                if (def.isFulfilled)
                    _onAuthComplete?.Invoke();
                else
                {
                    Debug.LogError(def.error.Message);
                    _onAuthFail?.Invoke();
                }
            }
            else
            {
                IEnumerator Routine(Deferred aDef)
                {
                    yield return aDef.Wait();

                    if(aDef.isFulfilled)
                        _onAuthComplete?.Invoke();
                    else
                    {
                        Debug.LogError(aDef.error.Message);
                        _onAuthFail?.Invoke();
                    }
                }

                StartCoroutine(routine: Routine(def));
            }
        }

        [Button]
        public void Unauthenticate()
        {
            GameAuthSdk.Unauthenticate();
        }

        [Button]
        public void UpdateDisplayName()
        {
            UpdateDisplayName(_displayName);
        }

        public void UpdateDisplayName(string displayName, Action onComplete = null, Action onFail = null)
        {
            _authService.AuthenticateToPhotonAfterLogin = true;

            _authService.UpdateDisplayName(displayName,
                (r) =>
                {
                    Debug.Log($"DisplayName updated to: {r.DisplayName}");
                    onComplete?.Invoke();
                }, (e) =>
                {
                    Debug.LogError(e.ErrorMessage);
                    onFail?.Invoke();
                });
        }

        public void GetDisplayName(Action<string> result)
        {
            _authService.GetDisplayName((r) =>
            {
                result?.Invoke(r);
            }, (e) =>
            {
                Debug.Log(e.ErrorMessage);
            });
        }
    }
}
