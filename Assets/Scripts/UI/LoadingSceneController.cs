using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PolyGo
{
    public class LoadingSceneController : MonoBehaviour
    {
        [SerializeField] private Slider sliderProgress;
        [SerializeField] private TextMeshProUGUI textProgress;
        [SerializeField] private float progressSpeed = 50f;
        [SerializeField] private ScreenFader screenFader;
        [SerializeField] private float delayWhileFading = 1;
        [SerializeField] private string sceneToLoad = "";

        private float displayProgress = 0f;

        void Start()
        {
            sliderProgress.value = 0f;
            textProgress.SetText("0%");

            StartCoroutine(LoadSceneAsync());
        }

        // Nos aseguramos de que se detengan todas las animaciones al salir de la escena para Dotween no de errores
        void OnDestroy()
        {
            DOTween.KillAll();
        }

        private IEnumerator LoadSceneAsync()
        {
            sceneToLoad = SceneManagerController.Instance.SceneToLoad;

            if (string.IsNullOrEmpty(sceneToLoad))
                sceneToLoad = "MainMenu";

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);

            // Desactivar la activación automática de la escena para que no se muestre hasta que se haya cargado completamente el fade out
            // cuando se pone al falso asyncOperation.progress se detine en 0.9f hasta que no sea verdadero
            asyncOperation.allowSceneActivation = false;

            // Espera mientras se carga la escena

            while (!asyncOperation.isDone)
            {
                float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f) * 100;

                displayProgress = Mathf.MoveTowards(displayProgress, progress, progressSpeed * Time.deltaTime);

                sliderProgress.value = displayProgress;
                textProgress.SetText($"{Mathf.RoundToInt(displayProgress)}%");

                // Si la carga de la escena está completa, desvanecer la pantalla de carga
                if (asyncOperation.progress >= 0.9f && displayProgress >= 100)
                {
                    if (screenFader != null)
                    {
                        screenFader.FadeIn();

                        yield return new WaitForSeconds(delayWhileFading);

                        DOTween.KillAll();
                        // Activa la escena precargada
                        asyncOperation.allowSceneActivation = true;
                    }
                }
                yield return null;
            }
        }
    }
}
