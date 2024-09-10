using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyGo
{
    public class Dot : MonoBehaviour
    {
        [SerializeField] private GameObject dotMeshPrefab;
        [SerializeField] private float scaleTime;
        [SerializeField] private float delay;
        [SerializeField] private Vector3 finalScale;
        [SerializeField] private bool autoRun;
        [SerializeField] private Ease ease = Ease.InOutQuad;

        void Start()
        {
            if (dotMeshPrefab != null)
                dotMeshPrefab.transform.localScale = Vector3.zero;

            if (autoRun)
                ShowDotMesh();
        }

        private void ShowDotMesh()
        {
            dotMeshPrefab.transform.DOScale(finalScale, scaleTime).SetDelay(delay).SetEase(ease);
        }
    }
}
