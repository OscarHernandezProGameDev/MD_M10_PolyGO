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
        [SerializeField]private int space;

        public bool IsMoving { get => _isMoving; set => _isMoving = value; }

        private void Move(Vector3 destinationPosition, float delay = 0.15f)
        {
            _isMoving = true;
            destination = destinationPosition;
            transform.DOMove(destinationPosition, moveSpeed).SetDelay(delay).SetEase(ease).OnComplete(() => OnCompleteMove(destinationPosition));
        }

        private void OnCompleteMove(Vector3 destinationPosition)
        {
            transform.position = destinationPosition;
            _isMoving = false;
        }

        public void MoveLeft()
        {
            Vector3 newPosition = transform.position + new Vector3(-space, 0, 0);

            Move(newPosition);
        }

        public void MoveRight()
        {
            Move(transform.position + new Vector3(space, 0, 0));
        }

        public void MoveForward()
        {
            Move(transform.position + new Vector3(0, 0, space));
        }

        public void MoveBackward()
        {
            Move(transform.position + new Vector3(0, 0, -space));
        }
    }
}