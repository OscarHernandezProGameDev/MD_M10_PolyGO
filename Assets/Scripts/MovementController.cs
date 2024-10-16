using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace PolyGo
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private protected Vector3 destination;
        [SerializeField] private protected bool faceDestination = false;
        [SerializeField] private protected Ease ease;
        [SerializeField] private bool _isMoving;

        public UnityEvent FinishMovementEvent;

        [SerializeField] private float moveSpeed;

        private Sequence sequence;

        private protected GridSystem gridSystem;
        protected Dot _currentDot;


        public bool IsMoving { get => _isMoving; set => _isMoving = value; }
        public Dot CurrentDot => _currentDot;

        public void MoveLeft()
        {
            Vector3 newPosition = transform.position + new Vector3(-GridSystem.spacing, 0, 0);

            Move(newPosition);
        }

        public void MoveRight()
        {
            Move(transform.position + new Vector3(GridSystem.spacing, 0, 0));
        }

        public void MoveForward()
        {
            Move(transform.position + new Vector3(0, 0, GridSystem.spacing));
        }

        public void MoveBackward()
        {
            Move(transform.position + new Vector3(0, 0, -GridSystem.spacing));
        }

        protected virtual void Awake()
        {
            gridSystem = FindFirstObjectByType<GridSystem>();
        }

        protected virtual void Start()
        {
            UpdateCurrentDot();
        }

        private protected void Move(Vector3 destinationPosition, float delay = 0.15f)
        {
            if (IsMoving)
                return;

            if (gridSystem != null)
            {
                Dot dotDestination = gridSystem.FindValidDot(destinationPosition);

                if (dotDestination != null && _currentDot != null)
                {
                    if (_currentDot.ConnectedDots.Contains(dotDestination))
                    {
                        OnBeforeMove();

                        _isMoving = true;
                        destination = destinationPosition;
                        if (faceDestination)
                            sequence.Append(RotateToDestination());

                        sequence.Append
                        (
                            transform.DOMove(destinationPosition, moveSpeed)
                            .SetDelay(delay)
                            .SetEase(ease)
                            .OnComplete(() => OnCompleteMove(destinationPosition))
                        );
                    }
                    else
                        Debug.Log($"Movement controller: {_currentDot.name} not connected {dotDestination.name}");
                }
            }
        }

        protected virtual void OnBeforeMove() { }

        protected virtual void OnCompleteMove(Vector3 destinationPosition)
        {
            transform.position = destinationPosition;
            _isMoving = false;
            UpdateCurrentDot();
        }

        protected void UpdateCurrentDot()
        {
            if (gridSystem != null)
                _currentDot = gridSystem.FindValidDot(transform.position);
        }

        protected virtual Tween RotateToDestination() => null;
    }
}
