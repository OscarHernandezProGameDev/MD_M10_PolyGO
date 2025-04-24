using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace PolyGo
{
    public class GraphicMover : MonoBehaviour
    {
        // Efecto pulso para el start button
        [Header("Pulse Effect")]
        [SerializeField] private RectTransform startButtonRectTransform;
        [SerializeField] private float pulseDuration = 0.5f;
        [SerializeField] private float pulseScale = 1.2f;
        [SerializeField] private float pulseDelay = 0.3f;
        [SerializeField] private Ease startButtonEase = Ease.InOutSine;

        // Efecto de movimiento para los elementos del endScreen
        [Header("Move Effect")]
        [SerializeField] private Transform startPosition;
        [SerializeField] private Transform endPosition;
        [SerializeField] private float moveTime;
        [SerializeField] private float moveDelay;
        [SerializeField] private Ease endSreenEase = Ease.OutBack;

        private Sequence pulseSequence;

        public void StartPulse()
        {
            if (startButtonRectTransform == null)
                return;

            pulseSequence = DOTween.Sequence();
            pulseSequence
                .Append(startButtonRectTransform.DOScale(pulseScale, pulseDuration / 2).SetEase(startButtonEase))
                .Append(startButtonRectTransform.DOScale(1f, pulseDuration / 2).SetEase(startButtonEase).SetDelay(pulseDelay))
                .SetLoops(-1);
        }

        public void StopPulse()
        {
            if (pulseSequence != null && pulseSequence.IsActive())
            {
                pulseSequence.Kill();
                startButtonRectTransform.localScale = Vector3.one; // Reset scale to original size
                pulseSequence = null;
            }
        }

        public void MoveFromTo()
        {
            Vector3 finalPosition = endPosition.position;

            transform.position = startPosition.position;
            transform
                .DOMove(finalPosition, moveTime)
                .SetDelay(moveDelay)
                .SetEase(endSreenEase);
        }

    }
}
