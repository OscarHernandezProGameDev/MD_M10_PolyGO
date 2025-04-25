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

        // => Gestión UI con Sound Manager para aplicar valores
        // => Guardar estos valores a traves de PlayerPrefs

        // Gestión opciones de vídeo

        // Mostrar controles en UI
        // => Info teclado + ratón / gamepad
        // => if (mobile)=> info controles táctiles

        public void SetGeneralVolume(float volume)
        {
            // Converter el valor del slider a decibelios y ajusta el volumen del grupo Master
            float adjustedVolume = volume * volume;

            myAudioMixer.SetFloat("MasterVolume", Mathf.Log10(adjustedVolume) * 20);
        }

        public void SetMusicVolume(float volume)
        {
            if (!musicToggle.isOn)
                return;

            float adjustedVolume = volume * volume;

            myAudioMixer.SetFloat("MusicVolume", Mathf.Log10(adjustedVolume) * 20);
        }

        public void SetSFXVolume(float volume)
        {
            if (!soundFXToggle.isOn)
                return;

            float adjustedVolume = volume * volume;

            myAudioMixer.SetFloat("SFXVolume", Mathf.Log10(adjustedVolume) * 20);
        }

        public void ToggleMusicOn(bool isOn)
        {
            if (isOn)
                myAudioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolumeSlider.value) * 20);
            else
                myAudioMixer.SetFloat("MusicVolume", -80f);
        }

        public void ToggleSFXOn(bool isOn)
        {
            if (isOn)
                myAudioMixer.SetFloat("SFXVolume", Mathf.Log10(soundFXSlider.value) * 20);
            else
                myAudioMixer.SetFloat("SFXVolume", -80f);
        }
        private void Start()
        {
            generalVolumeSlider.onValueChanged.AddListener(SetGeneralVolume);
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            soundFXSlider.onValueChanged.AddListener(SetSFXVolume);

            musicToggle.onValueChanged.AddListener(ToggleMusicOn);
            soundFXToggle.onValueChanged.AddListener(ToggleSFXOn);
        }
    }
}
