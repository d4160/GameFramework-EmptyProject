using d4160.Core.MonoBehaviours;
using UnityEngine;

namespace Game._Camera
{
    public class CameraManager : Singleton<CameraManager>
    {
        [SerializeField] private GameObject _firstPersonCameraRig;
        [SerializeField] private GameObject _thirdPersonCameraRig;

        public GameObject ThirdPersonCameraRig => _thirdPersonCameraRig;

        public void SetActiveFirstPerson(bool active)
        {
            _firstPersonCameraRig.SetActive(active);
            _thirdPersonCameraRig.SetActive(!active);
        }

        public void SetActiveThirdPerson(bool active)
        {
            _firstPersonCameraRig.SetActive(!active);
            _thirdPersonCameraRig.SetActive(active);
        }
    }
}
