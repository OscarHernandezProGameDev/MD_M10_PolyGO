using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace PolyGo
{
    public class PauseMenuController : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject pauseMenuUI;
        [SerializeField] private EndScreen EndScreen;

        private bool isPaused = false;

        public void PauseGame()
        {
            isPaused = true;
            pauseMenuUI.SetActive(true);
            EndScreen.EnableCameraBlur(true);
            Time.timeScale = 0f; // Congelamos el tiempo
        }

        public void ResumeGame()
        {
            isPaused = false;
            pauseMenuUI.SetActive(false);
            EndScreen.EnableCameraBlur(false);
            Time.timeScale = 1f; // Reanudamos el tiempo
        }

        public void QuitToMainMenu()
        {
            Time.timeScale = 1f; // nos aseguramos que el tiempo este Reanudado
            SceneManager.LoadScene("MainMenu");
        }

        public void QuitToDesktop()
        {
            Application.Quit();
        }

        private void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame || Gamepad.current?.startButton.wasPressedThisFrame == true)
            {
                if (isPaused)
                    ResumeGame();
                else
                    PauseGame();
            }
        }
    }
}
