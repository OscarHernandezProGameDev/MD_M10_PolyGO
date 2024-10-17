using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyGo
{
    public class EnemyDeath : MonoBehaviour
    {
        private GridSystem gridSystem;

        public Vector3 offscreenOffset = new Vector3(0, 10, 0);
        public float deathDelay = 0f;
        public float offscreenDelay = 1f;
        public Ease ease = Ease.InOutQuint;
        public float moveTime = 0.5f;
        public float delayTime = 0;

        public void MoveOffGrid(Vector3 target)
        {
            transform.DOMove(target, moveTime)
                .SetDelay(delayTime)
                .SetEase(ease);
        }

        public void Die() => StartCoroutine(DeathRoutine());

        private void Awake()
        {
            gridSystem = FindFirstObjectByType<GridSystem>();
        }

        private void Start()
        {
            Die();
        }

        IEnumerator DeathRoutine()
        {
            yield return new WaitForSeconds(deathDelay);

            Vector3 offscreenPos = transform.position + offscreenOffset;
            MoveOffGrid(offscreenPos);
            yield return new WaitForSeconds(moveTime + offscreenDelay);

            if (gridSystem.capturePositions.Count != 0 && gridSystem.CurrentCapturePosition <= gridSystem.capturePositions.Count)
            {
                Vector3 capturePos = gridSystem.capturePositions[gridSystem.CurrentCapturePosition].position;

                transform.position = capturePos + offscreenOffset;
                MoveOffGrid(capturePos);
                yield return new WaitForSeconds(moveTime);

                gridSystem.CurrentCapturePosition++;
                gridSystem.CurrentCapturePosition = Mathf.Clamp(gridSystem.CurrentCapturePosition, 0, gridSystem.capturePositions.Count - 1);
            }
        }
    }
}
