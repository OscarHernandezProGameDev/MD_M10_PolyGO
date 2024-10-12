using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyGo
{
    [RequireComponent(typeof(EnemyController))]
    [RequireComponent(typeof(EnemySensor))]

    public class EnemyManager : TurnManager
    {
        private EnemyController enemyController;
        private EnemySensor enemySensor;
        private GridSystem gridSystem;

        protected override void Awake()
        {
            base.Awake();
            enemyController = GetComponent<EnemyController>();
            enemySensor = GetComponent<EnemySensor>();
            gridSystem = FindFirstObjectByType<GridSystem>();
        }
    }
}
