using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyGo
{
    public class EnemyController : MovementController
    {
        [SerializeField] private float rotationTime = 0.5f;

        protected override void Awake()
        {
            base.Awake();
            faceDestination = true;
        }

        protected override void Start()
        {
            base.Start();
            //StartCoroutine(TestMoveRoutine());
        }

        protected override Tween RotateToDestination()
        {
            Vector3 relativeDirection = destination - transform.position;
            Quaternion newRotation = Quaternion.LookRotation(relativeDirection);
            float newY = newRotation.eulerAngles.y;

            return transform.DORotate(new Vector3(0, newY, 0), rotationTime, RotateMode.FastBeyond360).SetEase(ease);
        }

        private IEnumerator TestMoveRoutine()
        {
            yield return new WaitForSeconds(5f);
            MoveForward();
            yield return new WaitForSeconds(2f);
            MoveRight();
            yield return new WaitForSeconds(2f);
            MoveLeft();
            yield return new WaitForSeconds(2f);
            MoveBackward();
            yield return new WaitForSeconds(2f);
            MoveBackward();
        }
    }
}
