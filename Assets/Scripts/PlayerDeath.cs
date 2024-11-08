using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace PolyGo
{
    public class PlayerDeath : MonoBehaviour
    {
        [SerializeField] private Vector3 deathRotation = new Vector3(90, 0, 0);
        [SerializeField] private float duration = 0.3f;
        [SerializeField] private Ease ease = Ease.InOutBounce;

        public void Die()
        {
            transform.DORotate(deathRotation, duration).SetEase(ease);
        }
    }
}