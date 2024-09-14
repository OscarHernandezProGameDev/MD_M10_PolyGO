using System;
using System.Collections;
using System.Collections.Generic;
using PolyGo.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PolyGo
{
    public class GameManager : MonoBehaviour
    {
        private GridSystem gridSystem;
        private PlayerManager playerManager;

        private bool _hasLevelStarted;
        private bool _hasLevelFinished;
        private bool _isGamePlaying;
        private bool _isGamePaused;
        private bool _isGameOver;
        private float delay;

        public bool HasLevelStarted { get => _hasLevelStarted; set => _hasLevelStarted = value; }
        public bool HasLevelFinished { get => _hasLevelFinished; set => _hasLevelFinished = value; }
        public bool IsGamePlaying { get => _isGamePlaying; set => _isGamePlaying = value; }
        public bool IsGamePaused { get => _isGamePaused; set => _isGamePaused = value; }
        public bool IsGameOver { get => _isGameOver; set => _isGameOver = value; }

        public void PlayLevel()
        {
            _hasLevelStarted = true;
        }

        private void Awake()
        {
            gridSystem = FindFirstObjectByType<GridSystem>();
            playerManager = FindFirstObjectByType<PlayerManager>();
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

        IEnumerator PlayLevelRoutine()
        {
            _isGamePlaying = true;
            yield return new WaitForSeconds(delay);
            playerManager.playerInput.InputEnabled = true;

            while (!_isGameOver)
            {
                // Revisar condiciones de game over

                // Reviso si gano
                // Resultados

                // Revisar si pierdo
                // GameOver=true;

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
                // _hasLevelFinished = true;
                yield return null;
            }
            RestartNivel();
        }

        private void RestartNivel()
        {
            Scene scene = SceneManager.GetActiveScene();

            SceneManager.LoadScene(scene.name);
        }
    }
}
