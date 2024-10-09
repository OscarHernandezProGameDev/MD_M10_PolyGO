using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PolyGo.Player
{
    public class GatherInput : MonoBehaviour
    {
        private PlayerInput playerInput;
        private InputAction moveAction;

        private float _h;
        private float _v;
        private bool _inputEnabled;

        public float H => _h;
        public float V => _v;
        public bool InputEnabled { get => _inputEnabled; set => _inputEnabled = value; }

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            moveAction = playerInput.actions["Move"];
        }

        public void GetKeyInput()
        {
            if (_inputEnabled)
            {
                Vector2 move = moveAction.ReadValue<Vector2>();

                _h = move.x;
                _v = move.y;
                /*
                 * Version Input
                _h = Input.GetAxisRaw("Horizontal");
                _v = Input.GetAxisRaw("Vertical");
                */
            }
        }
    }
}