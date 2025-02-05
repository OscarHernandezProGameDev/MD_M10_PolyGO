using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PolyGo
{
    public enum SelectableCharacters
    {
        Character1, Character2, Character3, Character4, Character5, Character6
    }

    public class CharacterSelectionManager : MonoBehaviour
    {
        [SerializeField] private Button confirmCharacterButton;
        [SerializeField] private Button selectLevelButton;
        [SerializeField] private Button characterSelectionLeftButton;
        [SerializeField] private Button characterSelectionRightButton;
        [SerializeField] private TextMeshProUGUI titleSceneText;
        [SerializeField] private TextMeshProUGUI currentCharacterNameText;
        [SerializeField] private SelectableCharacters selectedCharacter = SelectableCharacters.Character1;
        [SerializeField] private Transform selectableCharactersPosition;

        private int spaceBetweenCharacters = 5;

        public void ConfirmCharacter()
        {
            // Save selected character at PlayerPrefs
            PlayerPrefs.SetInt("SelectedCharacter", (int)selectedCharacter);
            PlayerPrefs.Save();

            // Disable confirm button
            confirmCharacterButton.gameObject.SetActive(false);

            // Enable select level button
            selectLevelButton.gameObject.SetActive(true);
        }

        public void CharSelectionLeft()
        {
            if (selectedCharacter > SelectableCharacters.Character1)
            {
                selectedCharacter--;
                UpdateCharacterPosition();
                UpdateCharacterName();
            }
        }

        public void CharSelectionRight()
        {
            if (selectedCharacter < SelectableCharacters.Character6)
            {
                selectedCharacter++;
                UpdateCharacterPosition();
                UpdateCharacterName();
            }
        }

        void Start()
        {
            // Get selected character from PlayerPrefs, if not exists, set default character1
            selectedCharacter = (SelectableCharacters)PlayerPrefs.GetInt("SelectedCharacter", (int)SelectableCharacters.Character1);

            // Disable select level button until you confirm character
            selectLevelButton.gameObject.SetActive(false);

            UpdateCharacterPosition();
            UpdateCharacterName();
        }

        private void UpdateCharacterPosition()
        {
            // Calculate new X position depending on selected character
            float newXPosition = -(int)selectedCharacter * spaceBetweenCharacters;

            // Move selectableCharactersPosition GO to new Position
            selectableCharactersPosition.DOLocalMoveX(newXPosition, 0.5f).SetEase(Ease.InOutQuad);
        }

        private void UpdateCharacterName()
        {
            // Update character name text
            currentCharacterNameText.text = selectedCharacter.ToString();
        }
    }
}
