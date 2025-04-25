using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace PolyGo
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField] private Button applyButton;

        [Header("Sound UI")]
        [SerializeField] private Slider generalVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider soundFXSlider;
        [SerializeField] private Toggle musicToggle;
        [SerializeField] private Toggle soundFXToggle;

        [SerializeField] private AudioMixer myAudioMixer;

        // Gestión opciones de vídeo

        // Mostrar controles en UI
        // => Info teclado + ratón / gamepad
        // => if (mobile)=> info controles táctiles

        public void SetGeneralVolume(float volume)
        {
            // Converter el valor del slider a decibelios y ajusta el volumen del grupo Master
            float adjustedVolume = volume * volume;

            myAudioMixer.SetFloat("MasterVolume", Mathf.Log10(adjustedVolume) * 20);

            ActivateApplyButton();
        }

        public void SetMusicVolume(float volume)
        {
            if (!musicToggle.isOn)
                return;

            float adjustedVolume = volume * volume;

            myAudioMixer.SetFloat("MusicVolume", Mathf.Log10(adjustedVolume) * 20);

            ActivateApplyButton();
        }

        public void SetSFXVolume(float volume)
        {
            if (!soundFXToggle.isOn)
                return;

            float adjustedVolume = volume * volume;

            myAudioMixer.SetFloat("SFXVolume", Mathf.Log10(adjustedVolume) * 20);

            ActivateApplyButton();
        }

        public void ToggleMusicOn(bool isOn)
        {
            if (isOn)
                myAudioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolumeSlider.value) * 20);
            else
                myAudioMixer.SetFloat("MusicVolume", -80f);

            ActivateApplyButton();
        }

        public void ToggleSFXOn(bool isOn)
        {
            if (isOn)
                myAudioMixer.SetFloat("SFXVolume", Mathf.Log10(soundFXSlider.value) * 20);
            else
                myAudioMixer.SetFloat("SFXVolume", -80f);

            ActivateApplyButton();
        }
        private void Start()
        {
            LoadSettings();

            generalVolumeSlider.onValueChanged.AddListener(SetGeneralVolume);
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            soundFXSlider.onValueChanged.AddListener(SetSFXVolume);

            musicToggle.onValueChanged.AddListener(ToggleMusicOn);
            soundFXToggle.onValueChanged.AddListener(ToggleSFXOn);

            applyButton.onClick.AddListener(SaveAllSettings);
        }

        private void SaveAllSettings()
        {
            // Audio Data a guardar
            PlayerPrefs.SetFloat("GeneralVolume", generalVolumeSlider.value);
            PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
            PlayerPrefs.SetFloat("SFXVolume", soundFXSlider.value);

            PlayerPrefs.SetInt("MusicOn", musicToggle.isOn ? 1 : 0);
            PlayerPrefs.SetInt("SFXOn", soundFXToggle.isOn ? 1 : 0);

            PlayerPrefs.Save();
            Debug.Log("Settings saved!!!");

            applyButton.gameObject.SetActive(false);
        }

        private void LoadSettings()
        {
            // Obtener los valores guardados
            generalVolumeSlider.value = PlayerPrefs.GetFloat("GeneralVolume", 1f);
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
            soundFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

            musicToggle.isOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
            soundFXToggle.isOn = PlayerPrefs.GetInt("SFXOn", 1) == 1;

            SetGeneralVolume(generalVolumeSlider.value);
            SetMusicVolume(musicVolumeSlider.value);
            SetSFXVolume(soundFXSlider.value);

            ToggleMusicOn(musicToggle.isOn);
            ToggleSFXOn(soundFXToggle.isOn);

            applyButton.gameObject.SetActive(false);
        }

        private void ActivateApplyButton()
        {
            if (!applyButton.gameObject.activeSelf)
                applyButton.gameObject.SetActive(true);
        }
    }
}
