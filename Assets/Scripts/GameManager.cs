using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using PolyGo.Player;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
        [SerializeField] private int currentLevel = 1;
        [SerializeField] private int totalLevels = 2;
        [SerializeField] private TextMeshProUGUI levelName;
        [SerializeField] private GameObject endOfPrototypeMessage;

        [SerializeField] private float delay;

        public UnityEvent setUpEvent;
        public UnityEvent startLevelEvent;
        public UnityEvent playLevelEvent;
        public UnityEvent endLevelEvent;
        public UnityEvent loseLevelEvent;

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

        public void RestartLevel()
        {
            DOTween.KillAll();

            Scene scene = SceneManager.GetActiveScene();

            SceneManager.LoadScene(scene.name);
        }

        public void ContinueNextLevel()
        {
            if (currentLevel < totalLevels)
            {
                currentLevel++;
                SceneManager.LoadScene($"Level {currentLevel}");
            }
            else
                ShowPrototypeMessage();
        }

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
            SoundManager.Instance.PlayMusic(SoundManager.Instance.gameMusic);

            if (gridSystem != null && playerManager != null)
            {
                StartCoroutine(RunGameLoop());
            }
        }

        private void OnDisable()
        {
            DOTween.KillAll();
        }

        private void ShowPrototypeMessage()
        {
            endOfPrototypeMessage.SetActive(true);
            StartCoroutine(GoToMainMenu());
        }

        private IEnumerator GoToMainMenu()
        {
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("MainMenu");
        }

        IEnumerator RunGameLoop()
        {
            yield return StartCoroutine(StartLevelRoutine());
            yield return StartCoroutine(PlayLevelRoutine());
            yield return StartCoroutine(EndLevelRoutine());
        }

        IEnumerator StartLevelRoutine()
        {
            setUpEvent?.Invoke();
            playerManager.playerInput.InputEnabled = false;
            while (!_hasLevelStarted)
            {
                // Esperamos hasta que se pulse el boton de start
                yield return null;
            }
            startLevelEvent?.Invoke();
        }

        IEnumerator EndLevelRoutine()
        {
            playerManager.playerInput.InputEnabled = false;

            endLevelEvent?.Invoke();

            levelName.text = $"Level {currentLevel}";

            while (!_hasLevelFinished)
            {
                // Boton de continuar al siguiente nivel
                // Actualizar nivel mas alto completado
                _hasLevelFinished = true;
                yield return null;
            }
        }

        IEnumerator PlayLevelRoutine()
        {
            _isGamePlaying = true;
            yield return new WaitForSeconds(delay);
            playerManager.playerInput.InputEnabled = true;
            playerArrow.SetArrows();

            playLevelEvent?.Invoke();

            while (!_isGameOver)
            {
                // Resultados
                yield return null;
                _isGameOver = IsWinner();
            }
        }

        IEnumerator LoseLevelRoutine()
        {
            IsGameOver = true;

            // para que se puedar ver como captura al player
            yield return new WaitForSeconds(1.5f);
            loseLevelEvent?.Invoke();

            yield return new WaitForSeconds(1.5f);


            RestartLevel();
        }

        private bool IsWinner()
        {
            if (gridSystem.ActivePlayerDot != null && gridSystem.FinalTargetDot != null)
                // si el dot del player es el mismo que finalTargetDoc, devolvermos true
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