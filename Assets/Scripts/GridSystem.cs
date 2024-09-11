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

        public List<Dot> AllDots => _allDots;

        public Dot FindValidDot(Vector3 destinationPosition)
        {
            Vector2 gridPosition = Tools.Utilities.Vector2Round(new Vector2(destinationPosition.x, destinationPosition.z));

            return _allDots.Find(dot => dot.DotPosition == gridPosition);
        }

        private void Awake()
        {
            GetDotList();
        }

        private void GetDotList()
        {
            Dot[] dotsArray = FindObjectsOfType<Dot>();

            _allDots.AddRange(dotsArray);
        }
    }
}
