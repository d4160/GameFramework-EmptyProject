using UnityEngine.InputSystem;

namespace Game.Character
{
	/// <summary>
	/// Implementation of the First Person input
	/// </summary>
	public class FirstPersonInput : CharacterInput
	{
        // Tracks whether the character is crouching or not
		bool m_IsCrouching;

		/// <summary>
		/// Resets the input states
		/// </summary>
		/// <remarks>used by the <see cref="StandardAssets.Characters.FirstPerson.FirstPersonBrain"/> to reset inputs when entering the walking state</remarks>
		public override void ResetInputs()
		{
			m_IsCrouching = false;
			isSprinting = false;
		}

		/// <summary>
		/// Handles the sprint input
		/// </summary>
		/// <param name="context">context is required by the performed event</param>
		public override void OnSprint(InputAction.CallbackContext context)
		{
			// Since in the asset is set for press and release
            if (context.performed)
            {
                base.OnSprint(context);
                m_IsCrouching = false;
            }
        }
		
		/// <summary>
		/// Handles the crouch input
		/// </summary>
		/// <param name="context">context is required by the performed event</param>
		public override void OnCrouch(InputAction.CallbackContext context)
		{
            if (context.performed || context.canceled)
            {
                BroadcastInputAction(ref m_IsCrouching, crouchStarted, crouchEnded);
                isSprinting = false;
            }
        }
    }
}