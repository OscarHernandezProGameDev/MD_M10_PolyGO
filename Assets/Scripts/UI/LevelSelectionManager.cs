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
        [SerializeField] private Button playLevelButton;
        [SerializeField] private Button levelSelectionLeftButton;
        [SerializeField] private Button levelSelectionRightButton;
        [SerializeField] private TextMeshProUGUI titleSceneText;
        [SerializeField] private TextMeshProUGUI currentLevelNameText;
        [SerializeField] private Levels selectedLevel = Levels.Level1;
        [SerializeField] private Transform selectableLevelsPosition;
        [SerializeField] private SceneLoader sceneLoader;

        private int spaceBetweenCharacters = 5;
        private int highestUnlockedLevel = 0;

        public void PlayLevel()
        {
            sceneLoader.LoadScene(selectedLevel.ToString());
        }

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
            // Get the highest unlocked level from PlayPrefs, it's null set to level 1
            highestUnlockedLevel = PlayerPrefs.GetInt("HighestUnlockedLevel", 1);

            UpdateLevelPosition();
            UpdateLevelName();
            UpdatePlayButtonState();
        }

        private void UpdateLevelPosition()
        {
            // Calculate new X position depending on selected character
            float newXPosition = -(int)selectedLevel * spaceBetweenCharacters;

            // Move selectableCharactersPosition GO to new Position
            selectableLevelsPosition.DOLocalMoveX(newXPosition, 0.5f).SetEase(Ease.InOutQuad);
        }

        private void UpdateLevelName()
        {
            // Update character name text
            currentLevelNameText.text = selectedLevel.ToString();
        }

        private void UpdatePlayButtonState()
        {
            // Check if the select level is unlocked and enabled/disable play button depending on it
            if ((int)selectedLevel + 1 <= highestUnlockedLevel)
                playLevelButton.interactable = true;
            else
                playLevelButton.interactable = false;
        }
    }
}
