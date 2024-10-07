using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyGo.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Ease ease;
        [SerializeField] private Vector3 destination;
        [SerializeField] private bool _isMoving;

        private GridSystem gridSystem;
        private PlayerArrow playerArrow;

        public bool IsMoving { get => _isMoving; set => _isMoving = value; }

        void Awake()
        {
            gridSystem = FindFirstObjectByType<GridSystem>();
            playerArrow = GameObject.Find("PlayerArrow").GetComponent<PlayerArrow>();
        }

        private void Start()
        {
            UpdateGrid();
        }

        private void Move(Vector3 destinationPosition, float delay = 0.15f)
        {
            if (gridSystem != null)
            {
                Dot dotDestination = gridSystem.FindValidDot(destinationPosition);

                if (dotDestination != null && gridSystem.ActivePlayerDot.ConnectedDots.Contains(dotDestination))
                {
                    playerArrow.StopArrowsMovement();
                    _isMoving = true;
                    destination = destinationPosition;
                    transform.DOMove(destinationPosition, moveSpeed)
                        .SetDelay(delay)
                        .SetEase(ease)
                        .OnComplete(() => OnCompleteMove(destinationPosition));
                }
            }
        }

        private void OnCompleteMove(Vector3 destinationPosition)
        {
            transform.position = destinationPosition;
            _isMoving = false;
            UpdateGrid();
            playerArrow.ShowActiveArrows();
        }

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

        private void UpdateGrid()
        {
            if (gridSystem != null)
                gridSystem.UpdateActivePlayerDot();
        }
    }
}