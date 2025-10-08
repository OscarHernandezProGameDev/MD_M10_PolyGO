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
        [SerializeField] GameObject[] characters;

        // Otros

        private int selectedCharacter;
        private string selectedCharacterKey = "SelectedCharacter";

        void Start()
        {
            SoundManager.Instance.PlayMusic(SoundManager.Instance.menuMusic);

            // Tome el valor de la selección de personaje de PlayerPrefs, si no existe, establezca el personaje predeterminado Character1 (con indice 0)
            selectedCharacter = PlayerPrefs.GetInt(selectedCharacterKey, 0);
            ShowSelectedCharacter();
        }

        private void ShowSelectedCharacter()
        {
            for (int i = 0; i < characters.Length; i++)
            {
                if (i == selectedCharacter)
                    characters[i].SetActive(true);
                else
                    characters[i].SetActive(false);
            }
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
