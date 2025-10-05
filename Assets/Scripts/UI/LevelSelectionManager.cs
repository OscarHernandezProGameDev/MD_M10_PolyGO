using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

namespace PolyGo
{
    public class LevelSelectionManager : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button playLevelButton;
        [SerializeField] private Button levelSelectionLeftButton;
        [SerializeField] private Button levelSelectionRightButton;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI titleSceneText;
        [SerializeField] private TextMeshProUGUI currentLevelNameText;

        // Selección de nivel
        [Header("Level Selection")]
        [SerializeField] private GameObject[] selectableLevels;
        [SerializeField] private Transform selectableLevelsPosition;

        [Header("Scene Loader")]
        [SerializeField] private SceneLoader sceneLoader;

        private int selectedLevel = 0;
        private int highestUnlockedLevel = 0;
        private int spaceBetweenCharacters = 5;
        private string highestUnlockedKey = "HighestUnlockedLevel";

        public void PlayLevel()
        {
            sceneLoader.LoadScene(selectableLevels[selectedLevel - 1].name);
        }

        // Métodos para ir a traves de los niveles de las fechas
        public void LevelSelectionLeft()
        {
            if (selectedLevel > 1)
            {
                selectedLevel--;
                UpdateLevelPosition();
                UpdateLevelName();
                UpdatePlayButtonState();
            }
        }

        public void LevelSelectionRight()
        {
            if (selectedLevel < selectableLevels.Length)
            {
                selectedLevel++;
                UpdateLevelPosition();
                UpdateLevelName();
                UpdatePlayButtonState();
            }
        }

        private void Start()
        {
            // Toma el nível mas alto debloqueado desde PlayPrefs, si es nulo configuralo al nível 1
            highestUnlockedLevel = PlayerPrefs.GetInt(highestUnlockedKey, 1);
            selectedLevel = highestUnlockedLevel;

            UpdateLevelPosition();
            UpdateLevelName();
            UpdatePlayButtonState();
            UpdateLevelVisuals();
        }

        private void UpdateLevelPosition()
        {
            float newXPosition = -(selectedLevel - 1) * spaceBetweenCharacters;

            selectableLevelsPosition.DOLocalMoveX(newXPosition, 0.5f).SetEase(Ease.InOutQuad);
        }

        private void UpdateLevelName()
        {
            currentLevelNameText.text = selectableLevels[selectedLevel - 1].name;
        }

        private void UpdatePlayButtonState()
        {
            // Comprueba si el nível seleccionado esta desbloqueado y activa/desactiva el playbutton en consecuencia
            if (selectedLevel <= highestUnlockedLevel)
                playLevelButton.interactable = true;
            else
                playLevelButton.interactable = false;
        }

        private void UpdateLevelVisuals()
        {
            for(int i = 0; i < selectableLevels.Length; i++)
            {
                var levelGO = selectableLevels[i];

                if(levelGO)
                {
                    bool isUnlocked=(i+1) <= highestUnlockedLevel;

                    var lightsTransform = levelGO.transform.Find($"Lights Level {i + 1}");
                    var padlockTransform = levelGO.transform.Find("padlock");

                    if(lightsTransform)
                        lightsTransform.gameObject.SetActive(isUnlocked);

                    if(padlockTransform)
                    {
                        // El nivel 0 siempre estara desactivado
                        if(i==0)
                            padlockTransform.gameObject.SetActive(false);
                        else
                            padlockTransform.gameObject.SetActive(!isUnlocked);
                    }
                }
            }
        }
    }
}
