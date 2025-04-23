using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace PolyGo
{
    [RequireComponent(typeof(MaskableGraphic))]
    public class ScreenFader : MonoBehaviour
    {
        public Color solidColor = Color.white;
        public Color clearColor = new Color(1, 1, 1, 0);

        public float delay = 0.5f;
        public float timeToFade = 1f;
        public Ease easeType = Ease.OutExpo;

        private MaskableGraphic graphic;

        public void FadeOut()
        {
            graphic.DOColor(clearColor, timeToFade)
                .SetDelay(delay)
                .SetEase(easeType);
        }

        public void FadeIn()
        {
            graphic.DOColor(solidColor, timeToFade)
                .SetDelay(delay)
                .SetEase(easeType);
        }

        private void Awake()
        {
            graphic = GetComponent<MaskableGraphic>();
        }
    }
}
