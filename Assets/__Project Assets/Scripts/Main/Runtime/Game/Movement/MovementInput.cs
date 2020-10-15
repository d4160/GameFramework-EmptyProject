using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Movement
{
    [RequireComponent(typeof(MovementSystem))]
    public class MovementInput : MonoBehaviour
    {
        private MovementSystem _movementSystem;

        void Start()
        {
            _movementSystem = GetComponent<MovementSystem>();
        }

        public void Jump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                _movementSystem?.Jump();
            }
        }

        public void HorizontalMove(InputAction.CallbackContext ctx)
        {
            if (ctx.performed || ctx.canceled)
            {
                _movementSystem?.ChangeDirection((int)ctx.ReadValue<float>());
            }
        }
    }
}
