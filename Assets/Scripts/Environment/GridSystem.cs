using DG.Tweening;
using PolyGo.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyGo
{
    public class GridSystem : MonoBehaviour
    {
        [SerializeField] private GameObject finalTargetPreFab;
        [SerializeField] private float finalTargetTime = 2f;

        public static float spacing = 2;
        public static Vector2[] directions =
        {
            new Vector2(spacing, 0),
            new Vector2(-spacing, 0),
            new Vector2(0, spacing),
            new Vector2(0, -spacing),
        };

        private List<Dot> _allDots = new List<Dot>();
        private Dot _activePlayerDot;
        private Dot _finalTargetDot;
        private PlayerController playerController;
        private Ease finalTargetEase = Ease.InExpo;

        private int _currentCapturePosition = 0;

        public List<Transform> capturePositions;
        public float capturePositionIconSize = 0.5f;
        public Color capturePositionIconColor = Color.blue;

        public List<Dot> AllDots => _allDots;
        public Dot ActivePlayerDot => _activePlayerDot;

        public Dot FinalTargetDot => _finalTargetDot;

        public int CurrentCapturePosition { get => _currentCapturePosition; set => _currentCapturePosition = value; }

        public void InitGrid()
        {
            if (_activePlayerDot != null)
                _activePlayerDot.InitDot();
        }

        public Dot FindValidDot(Vector3 destinationPosition)
        {
            Vector2 gridPosition = Tools.Utilities.Vector2Round(new Vector2(destinationPosition.x, destinationPosition.z));

            return _allDots.Find(dot => dot.DotPosition == gridPosition);
        }

        public Dot FindActivePlayerDot()
        {
            if (playerController != null && !playerController.IsMoving)
                return FindValidDot(playerController.transform.position);

            return null;
        }

        public List<EnemyManager> FindAllEnemyAt(Dot dot)
        {
            var foundEnemies = new List<EnemyManager>();
            var enemies = FindObjectsOfType<EnemyManager>();

            foreach (var enemy in enemies)
            {
                var controller = enemy.GetComponent<EnemyController>();

                if (controller.CurrentDot == dot)
                    foundEnemies.Add(enemy);
            }

            return foundEnemies;
        }

        public Dot FindFinalTargetDot()
        {
            return _allDots.Find(dot => dot.IsFinalTarget);
        }

        public void UpdateActivePlayerDot()
        {
            _activePlayerDot = FindActivePlayerDot();
        }

        public void DrawFinalTargetDot()
        {
            GameObject finalTargetInstance = Instantiate(finalTargetPreFab, _finalTargetDot.transform.position, Quaternion.identity);

            finalTargetInstance.transform.DOScale(Vector3.one, finalTargetTime).SetEase(finalTargetEase);
        }

        private void Awake()
        {
            playerController = FindAnyObjectByType<PlayerController>();
            GetDotList();
            _finalTargetDot = FindFinalTargetDot();
        }

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 0, 1, 0.7f);
            if (_activePlayerDot != null)
                Gizmos.DrawSphere(_activePlayerDot.transform.position, 1);

            Gizmos.color = capturePositionIconColor;
            foreach (Transform capturePosition in capturePositions)
                Gizmos.DrawCube(capturePosition.position, Vector3.one * capturePositionIconSize);
        }

        private void GetDotList()
        {
            Dot[] dotsArray = FindObjectsOfType<Dot>();

            _allDots.AddRange(dotsArray);
        }
    }
}
