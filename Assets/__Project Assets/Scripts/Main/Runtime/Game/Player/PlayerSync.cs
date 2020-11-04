//using Game.Character;
//using Photon.Pun;
//using TMPro;
//using UnityEngine;

//namespace Game._Player
//{
//    [RequireComponent(typeof(CharacterAnimator))]
//    public class PlayerSync : MonoBehaviourPun, IPunObservable
//    {
//        [SerializeField] private float _distanceAwayToTeleport = 0.5f;
//        [SerializeField] private Renderer[] _renderers;
//        [SerializeField] private Canvas _characterCanvas;
//        [SerializeField] private TextMeshProUGUI _displayNameText;

//        private CharacterAnimator _remoteCharacterAnim;
//        private PlayerController _localPlayerController;
//        private Transform _remoteCachedTransform;

//        private float _lastSynchronizationTime = 0f;
//        private float _syncTime = 0f;
//        private float _syncDelay = 0f;

//        private float _forwardSpeed;
//        private float _verticalSpeed;
//        private float _lateralSpeed;
//        private float _turningSpeed;
//        private float _speedMultiplier;
//        private bool _onRightFoot;
//        private bool _strafe;

//        private Vector3 _startPos;
//        private Vector3 _endPos;

//        private Quaternion _startRot;
//        private Quaternion _endRot;

//        void Start()
//        {
//            _remoteCachedTransform = transform;

//            _remoteCharacterAnim = GetComponent<CharacterAnimator>();

//            SetVisibility(!photonView.IsMine);

//            PlayerManager.Instance.AddRemotePlayer(this);

//            UpdateNicknameText();
//        }

//        void OnDestroy()
//        {
//            if(PlayerManager.Instanced)
//                PlayerManager.Instance.RemoveRemotePlayer(this);
//        }

//        void Update()
//        {
//            if (!photonView.IsMine)
//            {
//                _syncTime += Time.deltaTime;
//                float synchValue = _syncTime / _syncDelay;

//                UpdateRemotePositionRotation(_remoteCachedTransform, _startPos, _endPos, _startRot, _endRot, synchValue);

//                this._remoteCharacterAnim.ForwardSpeed = _forwardSpeed;
//                this._remoteCharacterAnim.LateralSpeed = _lateralSpeed;
//                this._remoteCharacterAnim.VerticalSpeed = _verticalSpeed;
//                this._remoteCharacterAnim.TurningSpeed = _turningSpeed;
//                this._remoteCharacterAnim.SpeedMultiplier = _speedMultiplier;
//                this._remoteCharacterAnim.OnRightFoot = _onRightFoot;
//                this._remoteCharacterAnim.Strafe = _strafe;
//            }
//        }

//        public void SetPlayerController(PlayerController controller)
//        {
//            _localPlayerController = controller;
//        }

//        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
//        {
//            if (stream.IsWriting)
//            {
//                stream.SendNext(_localPlayerController.CachedTransform.position);
//                stream.SendNext(_localPlayerController.CachedTransform.rotation);

//                stream.SendNext(_localPlayerController.CharacterAnim.ForwardSpeed);
//                stream.SendNext(_localPlayerController.CharacterAnim.VerticalSpeed);
//                stream.SendNext(_localPlayerController.CharacterAnim.LateralSpeed);
//                stream.SendNext(_localPlayerController.CharacterAnim.TurningSpeed);

//                stream.SendNext(_localPlayerController.CharacterAnim.OnRightFoot);

//                // TODO Fall and Jumped

//                stream.SendNext(_localPlayerController.CharacterAnim.Strafe);

//                stream.SendNext(_localPlayerController.CharacterAnim.SpeedMultiplier);
//            }
//            else
//            {
//                _startPos = _remoteCachedTransform.position;
//                _endPos = (Vector3)stream.ReceiveNext();

//                _startRot = _remoteCachedTransform.rotation;
//                _endRot = (Quaternion)stream.ReceiveNext();

//                _forwardSpeed = (float)stream.ReceiveNext();
//                _verticalSpeed = (float)stream.ReceiveNext();
//                _lateralSpeed = (float)stream.ReceiveNext();
//                _turningSpeed = (float)stream.ReceiveNext();

//                _onRightFoot = (bool)stream.ReceiveNext();

//                // TODO Fall and Jumped

//                _strafe = (bool)stream.ReceiveNext();

//                _speedMultiplier = (float)stream.ReceiveNext();


//                _syncTime = 0f;
//                _syncDelay = Time.time - _lastSynchronizationTime;
//                _lastSynchronizationTime = Time.time;
//            }
//        }

//        private void UpdateRemotePositionRotation(Transform moveTransform, Vector3 startPosition, Vector3 endPosition, Quaternion startRotation, Quaternion endRotation, float syncValue)
//        {
//            float dist = Vector3.Distance(startPosition, endPosition);

//            // If far away just teleport there
//            if (dist > _distanceAwayToTeleport)
//            {
//                moveTransform.position = endPosition;
//                moveTransform.rotation = endRotation;
//            }
//            else
//            {
//                moveTransform.position = Vector3.Lerp(startPosition, endPosition, syncValue);
//                moveTransform.rotation = Quaternion.Lerp(startRotation, endRotation, syncValue);
//            }
//        }

//        private void SetVisibility(bool enabled)
//        {
//            for (var i = 0; i < _renderers.Length; i++)
//            {
//                _renderers[i].enabled = enabled;
//            }

//            _remoteCharacterAnim.Animator.enabled = enabled;
//            _characterCanvas.gameObject.SetActive(enabled);
//        }

//        private void UpdateNicknameText()
//        {
//            _displayNameText.text = photonView.Owner.NickName;
//        }
//    }
//}
