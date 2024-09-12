using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyGo
{
    public class Blocker : MonoBehaviour
    {
        BoxCollider2D boxCollider;
        [SerializeField] private Vector3 blockSize;

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.size = blockSize;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 1, 1, 0.7f);
            Gizmos.DrawCube(transform.position, blockSize);
        }
    }
}
