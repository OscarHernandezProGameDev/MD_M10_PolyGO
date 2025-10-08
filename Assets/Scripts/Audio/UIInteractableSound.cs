using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PolyGo
{
    public class UIInteractableSound : MonoBehaviour
    {
        [SerializeField] private AudioClip interactionSound; // Sonido de interacci�n asignado al componente.

        private void Awake()
        {
            // Detecta el componente asociado y a�ade el listener correspondiente.
            if (TryGetComponent(out Button button))
            {
                button.onClick.AddListener(PlaySound); // Sonido al hacer clic en un bot�n.
            }
            else if (TryGetComponent(out Slider slider))
            {
                slider.onValueChanged.AddListener(_ => PlaySound()); // Sonido al interactuar con un slider.
            }
            else if (TryGetComponent(out Toggle toggle))
            {
                toggle.onValueChanged.AddListener(_ => PlaySound()); // Sonido al cambiar un toggle.
            }
            else if (TryGetComponent(out TMPro.TMP_Dropdown tmpDropdown))
            {
                tmpDropdown.onValueChanged.AddListener(_ => PlaySound()); // Sonido al cambiar una opci�n en un Dropdown.
            }
        }

        private void PlaySound()
        {
            // Reproduce el sonido asignado mediante el SoundManager.
            if (interactionSound != null)
            {
                SoundManager.Instance.PlaySoundFX(interactionSound);
            }
        }
    }
}
