using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyGo
{
    public class EnemyController : MovementController
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            StartCoroutine(TestMoveRoutine());
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
