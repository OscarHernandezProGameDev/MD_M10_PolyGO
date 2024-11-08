using System;
using System.Collections;
using System.Collections.Generic;
using PolyGo.Player;
using UnityEngine;

namespace PolyGo
{
    public class EnemyAttack : MonoBehaviour
    {
        private PlayerManager playerManager;

        public void Attack()
        {
            playerManager?.Die();
        }

        private void Awake()
        {
            playerManager = FindObjectOfType<PlayerManager>();
        }
    }
}