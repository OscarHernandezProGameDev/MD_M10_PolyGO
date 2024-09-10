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

        private List<Dot> allDots = new List<Dot>();

        public List<Dot> AllDots => allDots;

        private void Awake()
        {
            GetDotList();
        }

        private void GetDotList()
        {
            Dot[] dotsArray = FindObjectsOfType<Dot>();

            allDots.AddRange(dotsArray);
        }
    }
}
