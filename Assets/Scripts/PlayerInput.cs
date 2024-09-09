using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyGo.Player
{
    public class PlayerInput : MonoBehaviour
    {
        private float _h;
        private float _v;
        private bool _inputEnabled;

        public float H => _h;
        public float V => _v;
        public bool InputEnabled { get => _inputEnabled; set => _inputEnabled = value; }

        public void GetKeyInput()
        {
            if (_inputEnabled)
            {
                _h = Input.GetAxisRaw("Horizontal");
                _v = Input.GetAxisRaw("Vertical");
            }
        }
    }
}