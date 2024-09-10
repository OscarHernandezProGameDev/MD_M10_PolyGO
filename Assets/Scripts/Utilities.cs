using UnityEngine;

namespace PolyGo.Tools
{
    public class Utilities
    {
        // crear la funcion Vector3Round
        public static Vector3 Vector3Round(Vector3 v)
        {
            return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
        }

        // ahora la de Vector2Round
        public static Vector2 Vector2Round(Vector2 v)
        {
            return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
        }

    }
}
