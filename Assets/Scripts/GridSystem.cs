using PolyGo.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyGo
{
    public class GridSystem : MonoBehaviour
    {
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
        private PlayerController playerController;

        public List<Dot> AllDots => _allDots;
        public Dot ActivePlayerDot => _activePlayerDot;

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

        public void UpdateActivePlayerDot()
        {
            _activePlayerDot = FindActivePlayerDot();
        }

        private void Awake()
        {
            playerController = FindAnyObjectByType<PlayerController>();
            GetDotList();
        }

        private void GetDotList()
        {
            Dot[] dotsArray = FindObjectsOfType<Dot>();

            _allDots.AddRange(dotsArray);
        }
    }
}
