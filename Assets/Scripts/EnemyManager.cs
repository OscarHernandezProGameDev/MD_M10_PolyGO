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

        private bool _isDead = false;

        public bool IsDead { get => _isDead; set => _isDead = value; }

        public void PlayTurn()
        {
            if (_isDead)
            {
                TurnFinish();

                return;
            }
            StartCoroutine(PlayTurnRoutine());
        }

        public void Die()
        {
            if (_isDead)
                return;

            _isDead = true;
        }

        protected override void Awake()
        {
            base.Awake();
            enemyController = GetComponent<EnemyController>();
            enemySensor = GetComponent<EnemySensor>();
            gridSystem = FindFirstObjectByType<GridSystem>();
        }

        private IEnumerator PlayTurnRoutine()
        {
            if (gameManager == null || gameManager.IsGameOver)
                yield break;

            // Detect Player
            enemySensor.UpdateSensor();

            // Wait
            // lo comentamos para que no se llame a los otros enemigos
            //yield return new WaitForSeconds(0f);

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

            yield return null;
        }
    }
}
