using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Combat
{
    [RequireComponent(typeof(CombatSystem))]
    public class CombatInput : MonoBehaviour
    {
        private CombatSystem _combat;

        void Start()
        {
            _combat = GetComponent<CombatSystem>();
        }

        public void Punch1(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                _combat.Attack(1);
            }
        }

        public void Punch2(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
                _combat.Attack(2);
        }

        public void Kick1(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
                _combat.Attack(3);
        }

        public void Kick2(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
                _combat.Attack(4);
        }
    }
}
