using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyGo
{
    public enum MovementType
    {
        Stationary,
        Patrol,
        Sentry
    }

    public class EnemyController : MovementController
    {
        [SerializeField] private float rotationTime = 0.5f;

        public Vector3 directionToMove = new Vector3(0, 0, GridSystem.spacing);
        public MovementType movementType = MovementType.Stationary;
        public float standTime = 1f;

        public void MoveOneTurn()
        {
            switch (movementType)
            {
                case MovementType.Stationary:
                    Stand();
                    break;
                case MovementType.Patrol:
                    Patrol();
                    break;
                case MovementType.Sentry:
                    Sentry();
                    break;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            faceDestination = true;
        }

        //protected override void Start()
        //{
        //    base.Start();
        //    //StartCoroutine(TestMoveRoutine());
        //}

        protected override Tween RotateToDestination()
        {
            Vector3 relativeDirection = destination - transform.position;
            Quaternion newRotation = Quaternion.LookRotation(relativeDirection);
            float newY = newRotation.eulerAngles.y;

            return transform.DORotate(new Vector3(0, newY, 0), rotationTime, RotateMode.FastBeyond360).SetEase(ease);
        }

        private void Stand() => StartCoroutine(StandRoutine());

        private IEnumerator StandRoutine()
        {
            yield return new WaitForSeconds(standTime);
            FinishMovementEvent.Invoke();
        }

        private void Patrol() => StartCoroutine(PatrolRoutine());

        private IEnumerator PatrolRoutine()
        {
            Vector2 dotPosition = _currentDot.DotPosition;
            Vector3 startPos = new Vector3(dotPosition.x, 0, dotPosition.y);
            Vector3 newPos = startPos + transform.TransformVector(directionToMove);
            Vector3 nextPos = startPos + transform.TransformVector(directionToMove * 2);

            Move(newPos, 0f);

            yield return new WaitWhile(() => IsMoving);

            if (gridSystem != null)
            {
                Dot newDestinationDot = gridSystem.FindValidDot(newPos);
                Dot nextDestinationDot = gridSystem.FindValidDot(nextPos);

                if (nextDestinationDot == null || !newDestinationDot.ConnectedDots.Contains(nextDestinationDot))
                {
                    destination = startPos;
                    RotateToDestination();
                    yield return new WaitForSeconds(rotationTime);
                }
            }

            FinishMovementEvent.Invoke();
        }

        private void Sentry() => StartCoroutine(SentryRoutine());

        private IEnumerator SentryRoutine()
        {
            Vector3 localForward = new Vector3(0, 0, GridSystem.spacing);
            destination = transform.TransformVector(localForward * -1) + transform.position;
            RotateToDestination();
            yield return new WaitForSeconds(rotationTime);

            FinishMovementEvent.Invoke();
        }
    }
}
