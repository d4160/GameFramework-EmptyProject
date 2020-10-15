using d4160.Core;
using Game.Character;
using UnityEngine;
using UnityEngine.GameFoundation;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private CharacterSystem _character;

        public CharacterSystem Character => _character;

        public void SwitchCurrentControlScheme(string controlScheme)
        {
            _playerInput?.SwitchCurrentControlScheme(controlScheme);
        }

        public void Start()
        {

            _character.InstantiateCharacter();
        }
    }
}
