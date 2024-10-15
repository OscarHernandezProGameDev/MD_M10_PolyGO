using System;
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

        public void PlayTurn() => StartCoroutine(PlayTurnRoutine());

        protected override void Awake()
        {
            base.Awake();
            enemyController = GetComponent<EnemyController>();
            enemySensor = GetComponent<EnemySensor>();
            gridSystem = FindFirstObjectByType<GridSystem>();
        }

        private IEnumerator PlayTurnRoutine()
        {
            if (gameManager == null)
                yield break;

            // Detect Player
            enemySensor.UpdateSensor();

            // Wait
            yield return new WaitForSeconds(0f);

            if (enemySensor.FoundPlayer)
            {
                // Attack Player
                // Notify GameManager to lose level
                gameManager.LoseLevel();
            }
            else
            {
                // Movement
                enemyController.MoveOneTurn();
            }
        }
    }
}
