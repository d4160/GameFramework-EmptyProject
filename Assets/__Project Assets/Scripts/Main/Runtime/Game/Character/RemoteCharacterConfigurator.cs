//using Game._Player;
//using NaughtyAttributes;
//using Photon.Pun;
//using Photon.Voice.PUN;
//using Photon.Voice.Unity;
//using StandardAssets.Characters.Common;
//using StandardAssets.Characters.Physics;
//using StandardAssets.Characters.ThirdPerson;
using UnityEngine;
//using UnityEngine.Audio;

namespace Game.Character
{
    public class RemoteCharacterConfigurator : MonoBehaviour
    {
        //[SerializeField] private GameObject _thirdPersonCharacter;
        //[SerializeField] private GameObject _headTransform;
        //[SerializeField] private AudioMixerGroup _voiceMixerGroup;

        //[Button]
        //public void Configure()
        //{
        //    DestroyImmediate(_thirdPersonCharacter.GetComponent<CinemachineInputGainDampener>());
        //    DestroyImmediate(_thirdPersonCharacter.GetComponent<ThirdPersonBrain>());
        //    DestroyImmediate(_thirdPersonCharacter.GetComponent<OpenCharacterController>());
        //    DestroyImmediate(_thirdPersonCharacter.GetComponent<ThirdPersonInput>());
            
        //    _thirdPersonCharacter.AddComponent<PlayerSync>();
        //    _thirdPersonCharacter.AddComponent<PhotonView>();
        //    var voiceView = _thirdPersonCharacter.AddComponent<PhotonVoiceView>();

        //    _thirdPersonCharacter.GetComponent<Animator>().applyRootMotion = false;

        //    var go = new GameObject("Speaker");
            
        //    var aSource = go.AddComponent<AudioSource>();
        //    aSource.playOnAwake = false;
        //    aSource.loop = false;
        //    aSource.spatialBlend = 1f;
        //    aSource.dopplerLevel = 1f;
        //    aSource.spread = 0f;
        //    aSource.minDistance = 3;
        //    aSource.maxDistance = 50;
        //    aSource.outputAudioMixerGroup = _voiceMixerGroup;
        //    go.transform.SetParent(_headTransform.transform, false);

        //    var speaker = go.AddComponent<Speaker>();

        //    voiceView.SpeakerInUse = speaker;
        //    voiceView.UsePrimaryRecorder = true;
        //    voiceView.SetupDebugSpeaker = true;
        //}
    }
}
