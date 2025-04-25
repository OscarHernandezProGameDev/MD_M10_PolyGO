using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

namespace PolyGo
{
    public enum Levels
    {
        Level1, Level2, Level3, Level4, Level5
    }

    public class LevelSelectionManager : MonoBehaviour
    {
        // Gestión de botones
        [SerializeField] private Button playLevelButton;
        [SerializeField] private Button levelSelectionLeftButton;
        [SerializeField] private Button levelSelectionRightButton;

        // Gestión de textos
        [SerializeField] private TextMeshProUGUI titleSceneText;
        [SerializeField] private TextMeshProUGUI currentLevelNameText;

        // Selección de nivel
        [SerializeField] private Levels selectedLevel = Levels.Level1;
        [SerializeField] private Transform selectableLevelsPosition;
        [SerializeField] private SceneLoader sceneLoader;

        private int spaceBetweenCharacters = 5;
        private int highestUnlockedLevel = 0;

        public void PlayLevel()
        {
            sceneLoader.LoadScene(selectedLevel.ToString());
        }

        // Métodos para ir a traves de los niveles de las fechas
        public void LevelSelectionLeft()
        {
            if (selectedLevel > Levels.Level1)
            {
                selectedLevel--;
                UpdateLevelPosition();
                UpdateLevelName();
                UpdatePlayButtonState();
            }
        }

        public void LevelSelectionRight()
        {
            if (selectedLevel < Levels.Level5)
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
            highestUnlockedLevel = PlayerPrefs.GetInt("HighestUnlockedLevel", 1);

            UpdateLevelPosition();
            UpdateLevelName();
            UpdatePlayButtonState();
        }

        private void UpdateLevelPosition()
        {
            float newXPosition = -(int)selectedLevel * spaceBetweenCharacters;

            selectableLevelsPosition.DOLocalMoveX(newXPosition, 0.5f).SetEase(Ease.InOutQuad);
        }

        private void UpdateLevelName()
        {
            currentLevelNameText.text = selectedLevel.ToString();
        }

        private void UpdatePlayButtonState()
        {
            // Comprueba si el nível seleccionado esta desbloqueado y activa/desactiva el playbutton en consecuencia
            if ((int)selectedLevel + 1 <= highestUnlockedLevel)
                playLevelButton.interactable = true;
            else
                playLevelButton.interactable = false;
        }
    }
}
