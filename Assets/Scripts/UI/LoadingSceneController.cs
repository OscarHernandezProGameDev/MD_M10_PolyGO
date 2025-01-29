using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PolyGo
{
    public class LoadingSceneController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Image loadingIcon;
        [SerializeField] private float rotationDuration = 1f;
        [SerializeField] private float fadeDuration = 1f;
        [SerializeField] private string sceneToLoad = "";

        private Vector3 iconRotation = new Vector3(0, 0, -360f);

        void Start()
        {
            if (canvasGroup != null && loadingIcon != null)
            {
                canvasGroup.alpha = 1;
                RotationIcon();
                StartCoroutine(LoadSceneAsync());
            }
        }

        // Nos aseguramos de que se detengan todas las animaciones al salir de la escena para Dotween no de errores
        void OnDestroy()
        {
            if (canvasGroup != null || loadingIcon != null)
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
                // Si la carga de la escena está completa, desvanecer la pantalla de carga
                if (asyncOperation.progress >= 0.9f)
                {
                    yield return new WaitForSeconds(1f);
                    canvasGroup.DOFade(0, fadeDuration).OnComplete(() =>
                    {
                        DOTween.KillAll();
                        // Activa la escena precargada
                        asyncOperation.allowSceneActivation = true;
                    });
                }
                yield return null;
            }
        }

        private void RotationIcon()
        {
            loadingIcon.rectTransform.DORotate(iconRotation, rotationDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        }
    }
}
