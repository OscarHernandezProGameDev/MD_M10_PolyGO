using System;
using System.Collections;
using System.Collections.Generic;
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


        void Awake()
        {
            gridSystem = FindFirstObjectByType<GridSystem>();
            SetArrows();
        }

        private void SetArrows()
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
        }
    }
}
