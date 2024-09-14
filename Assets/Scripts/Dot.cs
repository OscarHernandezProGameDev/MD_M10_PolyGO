using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace PolyGo
{
    public class Dot : MonoBehaviour
    {
        [SerializeField] private GameObject dotMeshPrefab;
        [SerializeField] private GameObject lineMeshPrefab;
        [SerializeField] private float scaleTime;
        [SerializeField] private float delay;
        [SerializeField] private Vector3 finalScale;
        [SerializeField] private Ease ease = Ease.InOutQuad;
        [SerializeField] private LayerMask blockerLayer;
        [SerializeField] private bool isInittialized;
        [SerializeField] private bool _isFinalTarget;

        private Vector2 _dotPosition;
        private Transform m_Transform;
        private List<Dot> _dotsBrothers = new List<Dot>();
        private List<Dot> _connectedDots = new List<Dot>();
        private GridSystem gridSystem;

        public Vector2 DotPosition => Tools.Utilities.Vector2Round(_dotPosition);
        public List<Dot> DotsBrothers => _dotsBrothers;
        public List<Dot> ConnectedDots => _connectedDots;

        public bool IsFinalTarget { get => _isFinalTarget; set => _isFinalTarget = value; }

        private void Awake()
        {
            gridSystem = FindFirstObjectByType<GridSystem>();
            m_Transform = GetComponent<Transform>();
            _dotPosition = new Vector2(transform.position.x, transform.position.z);
        }

        void Start()
        {
            if (dotMeshPrefab != null)
            {
                dotMeshPrefab.transform.localScale = Vector3.zero;
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

        public void InitDot()
        {
            if (!isInittialized)
            {
                ShowDotMesh();
                StartCoroutine(InitDotCoroutine());
                isInittialized = true;
            }
        }

        private IEnumerator InitDotCoroutine()
        {
            yield return new WaitForSeconds(delay);

            foreach (var dot in _dotsBrothers)
            {
                if (!_connectedDots.Contains(dot))
                {
                    Blocker blocker = FindBlocker(dot);

                    if (blocker == null)
                    {
                        ConnectDotLines(dot);
                        dot.InitDot();
                    }
                }
            }
        }

        private Blocker FindBlocker(Dot dot)
        {
            Vector3 position = m_Transform.position;
            Vector3 direction = (dot.transform.position - position).normalized;
            RaycastHit hit;

            if (Physics.Raycast(position, direction, out hit, GridSystem.spacing, blockerLayer))
            {
                return hit.collider.gameObject.GetComponent<Blocker>();
            }
            return null;
        }

        private void ConnectDotLines(Dot dot)
        {
            if (lineMeshPrefab != null)
            {
                GameObject lineInstance = Instantiate(lineMeshPrefab, transform.position, Quaternion.identity);

                lineInstance.transform.parent = transform;

                Lineconnection lineConnection = lineInstance.GetComponent<Lineconnection>();

                if (lineConnection != null)
                    lineConnection.DrawLine(transform.position, dot.transform.position);

                if (!_connectedDots.Contains(dot))
                    _connectedDots.Add(dot);
                if (!dot.ConnectedDots.Contains(this))
                    dot.ConnectedDots.Add(this);
            }
        }
    }
}
