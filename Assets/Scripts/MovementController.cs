using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace PolyGo
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Ease ease;
        [SerializeField] private Vector3 destination;
        [SerializeField] private bool _isMoving;

        private protected GridSystem gridSystem;
        protected Dot currentDot;

        public bool IsMoving { get => _isMoving; set => _isMoving = value; }

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

                if (dotDestination != null && currentDot != null)
                {
                    if (currentDot.ConnectedDots.Contains(dotDestination))
                    {
                        OnBeforeMove();
                        _isMoving = true;
                        destination = destinationPosition;
                        transform.DOMove(destinationPosition, moveSpeed)
                            .SetDelay(delay)
                            .SetEase(ease)
                            .OnComplete(() => OnCompleteMove(destinationPosition));
                    }
                }
                else
                    Debug.Log($"Movement controller: {currentDot.name} not connected {dotDestination.name}");
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
                currentDot = gridSystem.FindValidDot(transform.position);
        }
    }
}
