using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace PolyGo.Player
{
    public class GatherInput : MonoBehaviour
    {
        [Header("Cursor Spriters")]
        [Tooltip("Sprite to use when right clicking")]
        public Texture2D cursorClocked;
        [Tooltip("Sprite to use as the default")]
        public Texture2D cursorUnClocked;

        private PlayerInput playerInput;
        private InputAction moveAction;
        private InputAction zoomAction;
        private InputAction rotateAction;

        private float _h;
        private float _v;
        private float _zoomValue;
        private Vector2 _rotateValue;
        private bool _isRotating;
        private bool _inputEnabled;

        public float H => _h;
        public float V => _v;
        public bool InputEnabled { get => _inputEnabled; set => _inputEnabled = value; }

        public float ZoomValue => _zoomValue;
        public Vector2 RotateValue => _rotateValue;
        public bool IsRotating => _isRotating;

        private void Awake()
        {
            playerInput = FindAnyObjectByType<PlayerInput>();
            moveAction = playerInput.actions["Move"];
            zoomAction = playerInput.actions["Zoom"];
            rotateAction = playerInput.actions["Rotate"];
        }

        private void OnEnable()
        {
            rotateAction.performed += OnRotatePerformed;
            rotateAction.canceled += OnRotateCanceled;
        }

        private void OnDisable()
        {
            rotateAction.performed -= OnRotatePerformed;
            rotateAction.canceled -= OnRotateCanceled;
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

                _zoomValue = zoomAction.ReadValue<float>();
            }
            else
            {
                _h = 0;
                _v = 0;
            }
        }

        private void OnRotatePerformed(InputAction.CallbackContext context)
        {
            _isRotating = true;
            Cursor.SetCursor(cursorClocked, Vector2.zero, CursorMode.Auto);
            _rotateValue = context.ReadValue<Vector2>();
        }

        private void OnRotateCanceled(InputAction.CallbackContext context)
        {
            _isRotating = false;
            Cursor.SetCursor(cursorUnClocked, Vector2.zero, CursorMode.Auto);
            _rotateValue = Vector2.zero;
        }
    }
}