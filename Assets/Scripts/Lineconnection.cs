using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace PolyGo
{
    public class Lineconnection : MonoBehaviour
    {
        [SerializeField] private GameObject lineMeshPrefab;
        [SerializeField] private float scaleTime;
        [SerializeField] private float delay;
        [SerializeField] private float lineTickness;
        [SerializeField] private float lineHeight;
        [SerializeField] private Ease ease = Ease.InOutQuad;

        void Start()
        {
            DrawLine(new Vector3(0, 0, 0), new Vector3(0, 0, 6));
        }

        public void DrawLine(Vector3 startPosition, Vector3 endPosition)
        {
            transform.localScale = new Vector3(lineTickness, lineHeight, 0);

            Vector3 lineDirection = endPosition - startPosition;
            float zScale = lineDirection.magnitude;

            Vector3 finalScale = new Vector3(lineTickness, lineHeight, zScale);
            transform.rotation = Quaternion.LookRotation(lineDirection);
            transform.position = startPosition;

            transform.DOScale(finalScale, scaleTime).SetDelay(delay).SetEase(ease);
        }
    }
}
