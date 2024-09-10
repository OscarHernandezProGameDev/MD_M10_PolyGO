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

        private Vector2 _dotPosition;
        private List<Dot> _dotsBrothers;
        private GridSystem gridSystem;

        public Vector2 DotPosition => Tools.Utilities.Vector2Round(_dotPosition);
        public List<Dot> DotsBrothers => _dotsBrothers;

        private void Awake()
        {
            gridSystem = FindFirstObjectByType<GridSystem>();
            _dotPosition = new Vector2(transform.position.x, transform.position.z);
        }

        void Start()
        {
            if (dotMeshPrefab != null)
            {
                dotMeshPrefab.transform.localScale = Vector3.zero;
                if (autoRun)
                    ShowDotMesh();
                if (gridSystem != null)
                    _dotsBrothers = FindDotsBrothers(gridSystem.AllDots);
            }
        }

        private void ShowDotMesh()
        {
            dotMeshPrefab.transform.DOScale(finalScale, scaleTime).SetDelay(delay).SetEase(ease);
        }

        private List<Dot> FindDotsBrothers(List<Dot> allDots)
        {
            List<Dot> result = new List<Dot>();

            foreach (var direction in GridSystem.directions)
            {
                Vector2 dotPostion = DotPosition + direction;
                Dot dotBrother = allDots.Find(db => db.DotPosition == dotPostion);

                if (dotBrother != null && !result.Contains(dotBrother))
                    result.Add(dotBrother);
            }

            return result;
        }
    }
}
