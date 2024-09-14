using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace PolyGo
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private Vector3 myRotation = new Vector3(0, 360, 0);
        [SerializeField] private float rotationSpeed = 10f;

        Transform myTransform;
        RotateMode rotateMode = RotateMode.FastBeyond360;
        Ease ease = Ease.Linear;

        void Start()
        {
            myTransform = GetComponent<Transform>();
            myTransform.DORotate(myRotation, rotationSpeed, rotateMode).SetEase(ease)
                .SetRelative()
                .SetLoops(-1)
                .SetEase(ease);
        }
    }
}
