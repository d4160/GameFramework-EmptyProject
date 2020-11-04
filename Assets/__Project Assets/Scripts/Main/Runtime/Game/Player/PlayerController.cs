//using d4160.Core;
//using ExitGames.Client.Photon;
using Game.Character;
//using Photon.Pun;
//using Photon.Voice.Unity;
//using TMPro;
using UnityEngine;
//using UnityEngine.GameFoundation;
//using UnityEngine.InputSystem;

namespace Game._Player
{
    [RequireComponent(typeof(CharacterAnimator))]
    public class PlayerController : MonoBehaviour
    {
        //[SerializeField] private PlayerSync _remotePlayer;
        //[SerializeField] private TextMeshProUGUI _nameText;

        //private CharacterAnimator _characterAnim;
        //private Transform _cachedTransform;

        //public const string PlayerIsMuted_Key = "M";
        //public const string PlayerGenre_Key = "G";

        //public Transform CachedTransform => _cachedTransform;
        //public CharacterAnimator CharacterAnim => _characterAnim;

        //void OnEnable()
        //{
        //    _cachedTransform = transform;
        //    _characterAnim = GetComponent<CharacterAnimator>();

        //    if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.CurrentRoom != null)
        //    {
        //        GameObject go = PhotonNetwork.Instantiate(_remotePlayer.name, transform.position, transform.rotation);
        //        var playerSync = go.GetComponent<PlayerSync>();

        //        playerSync.SetPlayerController(this);

        //        _nameText.text = PhotonNetwork.NickName;

        //        // { PlayerPhotonViewId_Key, playerSync.photonView.ViewID }
        //        // { PlayerUserId_Key, PhotonNetwork.LocalPlayer.UserId }

        //        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable() { { PlayerIsMuted_Key, !PlayerManager.Instance.Recorder.TransmitEnabled }, { PlayerGenre_Key, PlayerManager.Instance.LocalPlayerGenre } });

        //        PlayerManager.Instance.RegisterLocalPhotonView(playerSync);
        //    }
        //}
    }
}
