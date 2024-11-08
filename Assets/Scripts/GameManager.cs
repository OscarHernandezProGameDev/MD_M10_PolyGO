using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using PolyGo.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PolyGo
{
    [Serializable]
    public enum Turn
    {
        Player,
        Enemy
    }

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private float delay;

        private GridSystem gridSystem;

        private PlayerManager playerManager;
        private List<EnemyManager> enemyManagers;
        private Turn _currentTurn = Turn.Player;

        private PlayerArrow playerArrow;

        private bool _hasLevelStarted;
        private bool _hasLevelFinished;
        private bool _isGamePlaying;
        private bool _isGamePaused;
        private bool _isGameOver;

        public bool HasLevelStarted
        {
            get => _hasLevelStarted;
            set => _hasLevelStarted = value;
        }

        public bool HasLevelFinished
        {
            get => _hasLevelFinished;
            set => _hasLevelFinished = value;
        }

        public bool IsGamePlaying
        {
            get => _isGamePlaying;
            set => _isGamePlaying = value;
        }

        public bool IsGamePaused
        {
            get => _isGamePaused;
            set => _isGamePaused = value;
        }

        public bool IsGameOver
        {
            get => _isGameOver;
            set => _isGameOver = value;
        }

        public Turn CurrentTurn => _currentTurn;

        public void PlayLevel()
        {
            _hasLevelStarted = true;
        }

        public void LoseLevel() => StartCoroutine(LoseLevelRoutine());

        public void UpdateTurn()
        {
            switch (_currentTurn)
            {
                case Turn.Player:
                    if (playerManager.TurnCompleted && !AllEnemiesAreDead())
                        PlayEnemyTurn();
                    break;
                case Turn.Enemy:
                    if (IsEnemyTurnComplete())
                        PlayPlayerTurn();
                    break;
            }
        }

        private void Awake()
        {
            gridSystem = FindFirstObjectByType<GridSystem>();
            playerManager = FindFirstObjectByType<PlayerManager>();
            playerArrow = GameObject.Find("PlayerArrow").GetComponent<PlayerArrow>();
            enemyManagers = FindObjectsOfType<EnemyManager>().ToList();
        }

        private void Start()
        {
            if (gridSystem != null && playerManager != null)
            {
                StartCoroutine(RunGameLoop());
            }
        }

        IEnumerator RunGameLoop()
        {
            yield return StartCoroutine(StartLevelRoutine());
            yield return StartCoroutine(PlayLevelRoutine());
            yield return StartCoroutine(EndLevelRoutine());
        }

        IEnumerator StartLevelRoutine()
        {
            playerManager.playerInput.InputEnabled = false;
            while (!_hasLevelStarted)
            {
                // Mostrar cartel inicio
                // Mostrar botón start
                //_hasLevelStarted = true;
                yield return null;
            }
        }

        IEnumerator EndLevelRoutine()
        {
            playerManager.playerInput.InputEnabled = false;
            // Mostrar pantalla fin de nivel

            while (!_hasLevelFinished)
            {
                // Boton de continuar
                Debug.Log("YOU WIN!!");
                _hasLevelFinished = true;
                yield return null;
            }

            RestartNivel();
        }

        private void RestartNivel()
        {
            DOTween.KillAll();

            Scene scene = SceneManager.GetActiveScene();

            SceneManager.LoadScene(scene.name);
        }

        IEnumerator PlayLevelRoutine()
        {
            _isGamePlaying = true;
            gridSystem.InitGrid();
            gridSystem.DrawFinalTargetDot();
            yield return new WaitForSeconds(delay);
            playerManager.playerInput.InputEnabled = true;
            playerArrow.SetArrows();

            while (!_isGameOver)
            {
                // Revisar condiciones de game over

                // Reviso si gano
                // Resultados

                // Revisar si pierdo
                // GameOver=true;

                yield return null;
                _isGameOver = IsWinner();
            }
        }

        IEnumerator LoseLevelRoutine()
        {
            IsGameOver = true;

            yield return new WaitForSeconds(2f);

            Debug.Log("YOU LOSE!!");

            RestartNivel();
        }

        private bool IsWinner()
        {
            if (gridSystem.ActivePlayerDot != null && gridSystem.FinalTargetDot != null)
                return gridSystem.ActivePlayerDot == gridSystem.FinalTargetDot;

            return false;
        }

        private void PlayPlayerTurn()
        {
            _currentTurn = Turn.Player;
            playerManager.TurnCompleted = false;
            // allow Player to move
        }

        private void PlayEnemyTurn()
        {
            _currentTurn = Turn.Enemy;
            foreach (var enemy in enemyManagers)
                if (enemy != null && !enemy.IsDead)
                {
                    enemy.TurnCompleted = false;
                    enemy.PlayTurn();
                }
        }

        private bool IsEnemyTurnComplete()
        {
            foreach (var enemy in enemyManagers)
            {
                if (enemy.IsDead)
                    continue;
                if (!enemy.TurnCompleted)
                    return false;
            }

            return true;
        }

        private bool AllEnemiesAreDead()
        {
            foreach (var enemy in enemyManagers)
            {
                if (!enemy.IsDead)
                    return false;
            }

            return true;
        }
    }
}