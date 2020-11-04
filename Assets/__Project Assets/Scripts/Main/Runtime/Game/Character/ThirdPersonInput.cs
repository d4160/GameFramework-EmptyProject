using System;
using Game.Character;
using StandardAssets.Characters.Common;
using StandardAssets.Characters.ThirdPerson;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Game.Character
{
	/// <summary>
	/// Implementation of the Third Person input
	/// </summary>
	public class ThirdPersonInput : CharacterInput, IThirdPersonInput
	{
		// Tracks if the character is strafing 
		bool m_IsStrafing;

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
            }
        }

		// Handles the recentre input 
		public override void OnRecentre(InputAction.CallbackContext context)
		{
			if(context.performed)
                recentreCamera?.Invoke();
        }

		// Handles the strafe input
		public override void OnStrafe(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				BroadcastInputAction(ref m_IsStrafing, strafeStarted, strafeEnded);
			}
		}
	}
}