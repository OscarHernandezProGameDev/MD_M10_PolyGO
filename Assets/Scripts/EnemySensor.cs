using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyGo
{
    public class EnemySensor : MonoBehaviour
    {
        public Vector3 directionToSearch = new Vector3(0, 0, 2);

        private GridSystem gridSystem;
        private Dot dotToSearch;
        private bool _foundPlayer;

        public bool FoundPlayer => _foundPlayer;

        public void UpdateSensor()
        {
            Vector3 worldSpacePositionToSearch = transform.TransformDirection(directionToSearch) + transform.position;

            if (gridSystem != null)
            {
                dotToSearch = gridSystem.FindValidDot(worldSpacePositionToSearch);
                if (dotToSearch == gridSystem.ActivePlayerDot)
                    _foundPlayer = true;
            }
        }

        void Awake()
        {
            gridSystem = FindFirstObjectByType<GridSystem>();
        }

        /*
        void Update()
        {
            UpdateSensor();
        }
        */
    }
}
