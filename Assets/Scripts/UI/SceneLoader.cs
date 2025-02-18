using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PolyGo
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeDuration = 0.2f;

        private void Start()
        {
            canvasGroup.alpha = 0;
        }

        public void LoadScene(string sceneName)
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
