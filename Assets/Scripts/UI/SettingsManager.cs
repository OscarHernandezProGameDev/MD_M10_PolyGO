using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace PolyGo
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField] private Button applyButton;

        [Header("Slider Sound UI")]
        [SerializeField] private Slider generalVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider soundFXSlider;

        [Header("Text Buttons Panel")]
        [SerializeField] private TextMeshProUGUI textButtonPanelAudio;
        [SerializeField] private TextMeshProUGUI textButtonPanelVideo;
        [SerializeField] private TextMeshProUGUI textButtonPanelControls;

        [Header("Toggle Sound UI")]
        [SerializeField] private Toggle musicToggle;
        [SerializeField] private Toggle soundFXToggle;
        [SerializeField] private Image toggleMusicImage;
        [SerializeField] private Image toggleSoundFXImage;
        [SerializeField] private TextMeshProUGUI toggleMusicText;
        [SerializeField] private TextMeshProUGUI toggleSoundFXText;
        [SerializeField] private Sprite spriteOn;
        [SerializeField] private Sprite spriteOff;

        [Header("Audio Mixer")]
        [SerializeField] private AudioMixer myAudioMixer;

        private Color normalColorButtonPanel;
        private Color selectedColorButtonPanel = Color.white;

        private float onPositionX = 8;
        private float offPositionX = -8;
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
            {
                myAudioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolumeSlider.value) * 20);
                toggleMusicImage.sprite = spriteOn;
                toggleMusicText.text = "ON";
                MoveToggleImage(onPositionX, toggleMusicImage);
            }
            else
            {
                myAudioMixer.SetFloat("MusicVolume", -80f);
                toggleMusicImage.sprite = spriteOff;
                toggleMusicText.text = "OFF";
                MoveToggleImage(offPositionX, toggleMusicImage);
            }

            ActivateApplyButton();
        }

        public void ToggleSFXOn(bool isOn)
        {
            if (isOn)
            {
                myAudioMixer.SetFloat("SFXVolume", Mathf.Log10(soundFXSlider.value) * 20);
                toggleSoundFXImage.sprite = spriteOn;
                toggleSoundFXText.text = "ON";
                MoveToggleImage(onPositionX, toggleSoundFXImage);
            }
            else
            {
                myAudioMixer.SetFloat("SFXVolume", -80f);
                toggleSoundFXImage.sprite = spriteOff;
                toggleSoundFXText.text = "OFF";
                MoveToggleImage(offPositionX, toggleSoundFXImage);
            }

            ActivateApplyButton();
        }

        public void ChangeColorTextButtonPanel(TextMeshProUGUI clickedText)
        {
            textButtonPanelAudio.color = normalColorButtonPanel;
            textButtonPanelVideo.color = normalColorButtonPanel;
            textButtonPanelControls.color = normalColorButtonPanel;

            clickedText.color = selectedColorButtonPanel;
        }
        private void Start()
        {
            LoadSettings();

            normalColorButtonPanel = textButtonPanelAudio.color;
            ChangeColorTextButtonPanel(textButtonPanelAudio);

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

            Debug.Log("Settings loaded!!!");

            applyButton.gameObject.SetActive(false);
        }

        private void ActivateApplyButton()
        {
            if (!applyButton.gameObject.activeSelf)
                applyButton.gameObject.SetActive(true);
        }

        private void MoveToggleImage(float positionX, Image image)
        {
            RectTransform rectTransform = image.GetComponent<RectTransform>();
            Vector2 anchoredPosition = rectTransform.anchoredPosition;

            anchoredPosition.x = positionX;
            rectTransform.anchoredPosition = anchoredPosition;
        }
    }
}
