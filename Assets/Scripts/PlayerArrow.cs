using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace PolyGo
{
    public class PlayerArrow : MonoBehaviour
    {
        private GridSystem gridSystem;
        private List<GameObject> arrows = new List<GameObject>();

        public GameObject arrowPreFab;
        public float scale = 1f;
        public float startOffset = 0.5f;
        public float endOffset = 1f;

        public void SetArrows()
        {
            foreach (Vector2 dir in GridSystem.directions)
            {
                Vector3 dirVector = new Vector3(dir.normalized.x, 0, dir.normalized.y);
                Quaternion rotation = Quaternion.LookRotation(dirVector);
                GameObject arrowInstance = Instantiate(arrowPreFab, transform.position + dirVector * startOffset, rotation);

                arrowInstance.transform.localScale = new Vector3(scale, scale, scale);
                arrowInstance.transform.parent = transform;

                arrows.Add(arrowInstance);
            }

            ShowActiveArros();
            MoveArrows();
        }

        public void ShowActiveArros()
        {
            if (gridSystem.ActivePlayerDot == null)
                return;

            for (int i = 0; i < GridSystem.directions.Length; i++)
            {
                Dot brother = gridSystem.ActivePlayerDot.FindDotBrothersAt(GridSystem.directions[i]);
                bool activeState = brother != null && brother == gridSystem.ActivePlayerDot.ConnectedDots.Contains(brother);

                arrows[i].SetActive(activeState);
            }

            MoveArrows();
        }

        public void StopArrowsMovement()
        {
            foreach (var arrow in arrows)
            {
                DOTween.Kill(arrow.transform);
                arrow.SetActive(false);
            }
        }

        void Awake()
        {
            gridSystem = FindFirstObjectByType<GridSystem>();
        }

        void MoveArrows()
        {
            foreach (var arrow in arrows)
                MoveArrow(arrow);
        }

        void MoveArrow(GameObject arrow)
        {
            arrow.transform.position = transform.position + arrow.transform.forward * startOffset;
            arrow.transform.DOMove
            (
                arrow.transform.position + arrow.transform.forward * endOffset,
                0.5f
            )
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InExpo);
        }
    }
}
