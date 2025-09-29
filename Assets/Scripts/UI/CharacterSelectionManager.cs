using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PolyGo
{
    public class CharacterSelectionManager : MonoBehaviour
    {
        // Gesti�n de botones
        [SerializeField] private Button confirmCharacterButton;
        [SerializeField] private Button selectLevelButton;
        [SerializeField] private Button characterSelectionLeftButton;
        [SerializeField] private Button characterSelectionRightButton;

        // Gesti�n de textos
        [SerializeField] private TextMeshProUGUI titleSceneText;
        [SerializeField] private TextMeshProUGUI currentCharacterNameText;

        // Selecci�n de personaje
        [SerializeField] private GameObject[] selectableCharacters;
        [SerializeField] private Transform selectableCharactersPosition;

        private int spaceBetweenCharacters = 5;
        private int selectedCharacter;
        private string selectedCharacterKey = "SelectedCharacter";

        public void ConfirmCharacter()
        {
            // Guardar el personaje seleccionado en PlayerPrefs
            PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
            PlayerPrefs.Save();

            // Desactivar el bot�n de confirmar el personaje
            confirmCharacterButton.gameObject.SetActive(false);

            // Activa el bot�n de selecci�n de nivel
            selectLevelButton.gameObject.SetActive(true);
        }

        public void CharSelectionLeft()
        {
            if (selectedCharacter > 0)
            {
                selectedCharacter--;
                UpdateCharacterPosition();
                UpdateCharacterName();
            }
        }

        public void CharSelectionRight()
        {
            if (selectedCharacter < selectableCharacters.Length - 1)
            {
                selectedCharacter++;
                UpdateCharacterPosition();
                UpdateCharacterName();
            }
        }

        void Start()
        {
            // Tome el valor de la selecci�n de personaje de PlayerPrefs, si no existe, establezca el personaje predeterminado Character1 (con indice 0)
            selectedCharacter = PlayerPrefs.GetInt(selectedCharacterKey, 0);

            // desactivar el bot�n de selecci�n de n�vel hasta que confirmeros personaje
            selectLevelButton.gameObject.SetActive(false);

            UpdateCharacterPosition();
            UpdateCharacterName();
        }

        private void UpdateCharacterPosition()
        {
            // Calcula la nueva en X depedendiendo del personaje seleccionado
            float newXPosition = -selectedCharacter * spaceBetweenCharacters;

            // Mueve el GO de "selectableCharactersPosition" hasta la nueva posici�n
            selectableCharactersPosition.DOLocalMoveX(newXPosition, 0.5f).SetEase(Ease.InOutQuad);
        }

        private void UpdateCharacterName()
        {
            // Actualiza los nombres de los personajes usando el enumerado "selectedCharacter"
            currentCharacterNameText.text = selectableCharacters[selectedCharacter].name;
        }
    }
}
