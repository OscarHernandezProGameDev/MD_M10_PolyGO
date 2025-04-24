using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace PolyGo
{
    public class GraphicMover : MonoBehaviour
    {
        [SerializeField] private RectTransform startButtonRectTransform;
        [SerializeField] private float pulseDuration = 0.5f;
        [SerializeField] private float pulseScale = 1.2f;
        [SerializeField] private float pulseDelay = 0.3f;
        [SerializeField] private Ease startButtonEase = Ease.InOutSine;

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
    }
}
