using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyGo
{
    public class Grid : MonoBehaviour
    {
        public static float spacing = 2;
        public static Vector2[] directions =
        {
            new Vector2(spacing, 0),
            new Vector2(-spacing, 0),
            new Vector2(0, spacing),
            new Vector2(0, -spacing),
        };
    }
}
