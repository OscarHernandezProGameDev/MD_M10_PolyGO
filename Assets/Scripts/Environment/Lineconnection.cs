using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace PolyGo
{
    public class Lineconnection : MonoBehaviour
    {
        [SerializeField] private float scaleTime;
        [SerializeField] private float delay;
        [SerializeField] private float lineTickness;
        [SerializeField] private float lineHeight;
        [SerializeField] private Ease ease = Ease.InOutQuad;

        Transform m_transform;

        void Awake()
        {
            m_transform = GetComponent<Transform>();
        }

        public void DrawLine(Vector3 startPosition, Vector3 endPosition)
        {
            m_transform.localScale = new Vector3(lineTickness, lineHeight, 0);

            Vector3 lineDirection = endPosition - startPosition;
            float zScale = lineDirection.magnitude;

            Vector3 finalScale = new Vector3(lineTickness, lineHeight, zScale);
            m_transform.rotation = Quaternion.LookRotation(lineDirection);
            m_transform.position = startPosition;

            m_transform.DOScale(finalScale, scaleTime).SetDelay(delay).SetEase(ease);
        }
    }
}
