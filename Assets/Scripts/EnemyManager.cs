using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PolyGo
{
    [RequireComponent(typeof(EnemyController))]
    [RequireComponent(typeof(EnemySensor))]
    [RequireComponent(typeof(EnemyAttack))]
    public class EnemyManager : TurnManager
    {
        public UnityEvent deathEvent;

        private EnemyController enemyController;
        private EnemySensor enemySensor;
        private EnemyAttack enemyAttack;
        private GridSystem gridSystem;

        private bool _isDead = false;

        public bool IsDead
        {
            get => _isDead;
            set => _isDead = value;
        }

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
            deathEvent?.Invoke();
        }

        protected override void Awake()
        {
            base.Awake();
            enemyController = GetComponent<EnemyController>();
            enemySensor = GetComponent<EnemySensor>();
            enemyAttack = GetComponent<EnemyAttack>();
            gridSystem = FindFirstObjectByType<GridSystem>();
        }

        private IEnumerator PlayTurnRoutine()
        {
            if (gameManager == null || gameManager.IsGameOver)
                yield break;

            // Detect Player
            enemySensor.UpdateSensor(enemyController.CurrentDot);

            // Wait
            // lo comentamos para que no se llame a los otros enemigos
            //yield return new WaitForSeconds(0f);

            if (enemySensor.FoundPlayer)
            {
                // Notify GameManager to lose level, lo ponemos antes del attack para que el game Manager cambie de estado
                gameManager.LoseLevel();

                Vector3 positionPlayer = new Vector3(gridSystem.ActivePlayerDot.DotPosition.x, 0,
                    gridSystem.ActivePlayerDot.DotPosition.y);

                // el enemigo se abalanza sobre el player
                enemyController.Move(positionPlayer, 0f);
                while (enemyController.IsMoving)
                    yield return null;

                // Attack Player
                enemyAttack.Attack();
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