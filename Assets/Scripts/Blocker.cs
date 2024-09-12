using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyGo
{
    public class Blocker : MonoBehaviour
    {
        BoxCollider boxCollider;
        [SerializeField] private Vector3 blockSize;

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider>();
            boxCollider.size = blockSize;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 1, 1, 0.7f);
            Gizmos.DrawCube(transform.position, blockSize);
        }
    }
}
