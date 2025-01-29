using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PolyGo
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeDuration = 0.2f;

        [SerializeField] private Button characterSelectionButton;
        [SerializeField] private Button levelSelectionButton;
        [SerializeField] private Button settingsButton;

        private void Start()
        {
            canvasGroup.alpha = 0;

            // es provisional, luego se pondra en el inspector
            characterSelectionButton.onClick.AddListener(() => LoadScene("CharacterSelection"));
            levelSelectionButton.onClick.AddListener(() => LoadScene("LevelSelection"));
            settingsButton.onClick.AddListener(() => LoadScene("Settings"));
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
