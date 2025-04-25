using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace PolyGo
{
    public class SettingsManager : MonoBehaviour
    {
        [Header("Sound UI")]
        [SerializeField] private Slider generalVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider soundFXSlider;
        [SerializeField] private Toggle musicToggle;
        [SerializeField] private Toggle soundFXToggle;

        [SerializeField] private AudioMixer myAudioMixer;

        // => Gesti�n UI con Sound Manager para aplicar valores
        // => Guardar estos valores a traves de PlayerPrefs

        // Gesti�n opciones de v�deo

        // Mostrar controles en UI
        // => Info teclado + rat�n / gamepad
        // => if (mobile)=> info controles t�ctiles

        public void SetGeneralVolume(float volume)
        {
            // Converter el valor del slider a decibelios y ajusta el volumen del grupo Master
            float adjustedVolume = volume * volume;

            myAudioMixer.SetFloat("MasterVolume", Mathf.Log10(adjustedVolume) * 20);
        }

        public void SetMusicVolume(float volume)
        {
        }

        public void SetSFXVolume(float volume)
        {

        }
    }
}
