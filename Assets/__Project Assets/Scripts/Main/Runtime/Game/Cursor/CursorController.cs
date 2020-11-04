using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game._Cursor
{
    public class CursorController : MonoBehaviour
    {
        [SerializeField] private CursorLockMode _cursorModeAtStart;

        void Start()
        {
            UnityEngine.Cursor.lockState = _cursorModeAtStart;
        } 

        [Button]
        public void LockCursor()
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }

        [Button]
        public void UnlockCursor()
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }

        [Button]
        public void ConfineCursor()
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
