using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyGo
{
    public class EnemyController : MovementController
    {
        [SerializeField] private float rotationTime = 0.5f;
        public float standTime = 1f;

        public void MoveOneTurn()
        {
            Stand();
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
    }
}
