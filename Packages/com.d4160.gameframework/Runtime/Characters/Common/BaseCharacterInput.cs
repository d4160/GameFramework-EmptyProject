using System;
using UnityEngine;

namespace StandardAssets.Characters.Common
{
    public abstract class BaseCharacterInput : MonoBehaviour
    {
        /// <summary>
        /// Fired when the jump input is pressed - i.e. on key down
        /// </summary>
        public Action jumpPressed;

        /// <summary>
        /// Fired when the sprint input is started
        /// </summary>
        public Action sprintStarted;

        /// <summary>
        /// Fired when the sprint input is disengaged
        /// </summary>
        public Action sprintEnded;

        /// <summary>
        /// Fired when the crouch is started
        /// </summary>
        public Action crouchStarted;

        /// <summary>
        /// Fired when the crouch is ended
        /// </summary>
        public Action crouchEnded;

        /// <summary>
        /// Fired when strafe input is started
        /// </summary>
        public Action strafeStarted;

        /// <summary>
        /// Fired when the strafe input is ended
        /// </summary>
        public Action strafeEnded;

        /// <summary>
		/// Fired when the recentre camera input is applied
		/// </summary>
		public Action recentreCamera;

        /// <summary>
        /// Look input axis sensitivity
        /// </summary>
        [Serializable]
        public struct Sensitivity
        {
            [SerializeField, Range(0.2f, 2f), Tooltip("Look sensitivity for mouse")]
            float m_MouseVertical;
            [SerializeField, Range(0.2f, 2f), Tooltip("Look sensitivity for mouse")]
            float m_MouseHorizontal;

            [SerializeField, Range(0.2f, 2f), Tooltip("Look sensitivity for analog gamepad stick")]
            float m_GamepadVertical;
            [SerializeField, Range(0.2f, 2f), Tooltip("Look sensitivity for analog gamepad stick")]
            float m_GamepadHorizontal;

            [SerializeField, Range(0.2f, 2f), Tooltip("Look sensitivity for on screen touch stick")]
            float m_TouchVertical;
            [SerializeField, Range(0.2f, 2f), Tooltip("Look sensitivity for on screen touch stick")]
            float m_TouchHorizontal;

            public float mouseVerticalSensitivity { get { return m_MouseVertical; } }

            public float mouseHorizontalSensitivity { get { return m_MouseHorizontal; } }

            public float gamepadVerticalSensitivity { get { return m_GamepadVertical; } }

            public float gamepadHorizontalSensitivity { get { return m_GamepadHorizontal; } }

            public float touchVerticalSensitivity { get { return m_TouchVertical; } }

            public float touchHorizontalSensitivity { get { return m_TouchHorizontal; } }
        }

        [SerializeField, Tooltip("Prefab of canvas used to render the on screen touch control graphics")]
        protected GameObject m_TouchControlsPrefab;

        [SerializeField, Tooltip("Vertical and Horizontal axis sensitivity")]
        protected Sensitivity m_CameraLookSensitivity;

        [SerializeField, Tooltip("Toggle the Cursor Lock Mode? Press ESCAPE during play mode to unlock")]
        protected bool m_CursorLocked = true;

        /// <summary>
        /// Invert the X axis input
        /// </summary>
        [Tooltip("Invert horizontal look direction?")]
        public bool m_InvertX;

        /// <summary>
        /// Invert the Y axis input
        /// </summary>
        [Tooltip("Invert vertical look direction?")]
        public bool m_InvertY;

        /// <summary>
        /// Gets if the movement input is being applied
        /// </summary>
        public bool hasMovementInput { get { return moveInput != Vector2.zero; } }

        /// <summary>
        /// Gets/sets the move input vector
        /// </summary>
        public Vector2 moveInput { get; protected set; }

        /// <summary>
        /// Gets whether or not the jump input is currently applied
        /// </summary>
        public bool HasJumpInput { get; protected set; }

        /// <summary>
		/// Rotation of a moving platform applied to the look input vector (so that the platform rotates the camera)
		/// </summary>
        public Vector2 MovingPlatformLookInput { get; set; }

        /// <summary>
        /// Used to check if the last look input came from a mouse
        /// </summary>
        public bool usingMouseInput => m_UsingMouseInput;

        /// <summary>
        /// Gets/sets the look input vector
        /// </summary>
        public Vector2 lookInput
        {
            get { return m_lookInput; }
            set { m_lookInput = value; }
        }

        /// <summary>
        /// The camera look sensitivity
        /// </summary>
        public Sensitivity cameraLookSensitivity
        {
            get { return m_CameraLookSensitivity; }
        }

        // Was the last look input from a mouse
        protected bool m_UsingMouseInput;

        // Look input vector
        protected Vector2 m_lookInput;

        /// <summary>
        /// Can be called to determine whether or not Touch Controls are being used (instead of Standard Controls)
        /// </summary>
        public bool UseTouchControls()
        {
            //Assume Touch Controls are wanted for iOS and Android builds
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
			return true;
#else
            return false;
#endif
        }

        public virtual void ResetInputs()
        {
        }
    }
}
