using Cinemachine;
using UI_Input;
using UnityEngine;

namespace Game._Camera
{
    public class CinemachineCameraInput : MonoBehaviour
    {
        [SerializeField] private Joystick _joyStick;


        void Start()
        {
            CinemachineCore.GetInputAxis = HandleAxisInputDelegate;
        }

        float HandleAxisInputDelegate(string axisName)
        {
            switch (axisName)
            {

                case "Mouse X":

                    return _joyStick.XAxis;

                case "Mouse Y":
                    return _joyStick.YAxis;

                default:
                    Debug.LogError("Input <" + axisName + "> not recognyzed.", this);
                    break;
            }

            return 0f;
        }
    }
}
