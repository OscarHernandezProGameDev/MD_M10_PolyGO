using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PolyGo
{
    public class ProvisionalTesting : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeDuration = 0.2f;

        [SerializeField] private Button backToMenuButton;

        private void Start()
        {
            canvasGroup.alpha = 0;
            backToMenuButton.onClick.AddListener(() => LoadScene("MainMenu"));
        }

        private void LoadScene(string sceneName)
        {
            SceneManagerController.Instance.SceneToLoad = sceneName;
            StartCoroutine(DoFadeAndLoadScene());
        }

        private IEnumerator DoFadeAndLoadScene()
        {
            yield return canvasGroup.DOFade(1f, fadeDuration).WaitForCompletion();
            SceneManager.LoadScene("Loading");
        }
    }
}
