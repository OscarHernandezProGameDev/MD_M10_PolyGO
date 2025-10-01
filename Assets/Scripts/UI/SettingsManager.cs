using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace PolyGo
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField] private Button applyButton;

        [Header("Text Buttons Panel")]
        [SerializeField] private TextMeshProUGUI textButtonPanelAudio;
        [SerializeField] private TextMeshProUGUI textButtonPanelVideo;
        [SerializeField] private TextMeshProUGUI textButtonPanelControls;

        [Header("Audio Sound UI")]
        [SerializeField] private Slider generalVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider soundFXSlider;
        [SerializeField] private Toggle musicToggle;
        [SerializeField] private Toggle soundFXToggle;

        [Header("Video Options UI")]
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private TMP_Dropdown qualityDropdown;
        [SerializeField] private Toggle fullscreenFXToggle;

        [Header("Audio Mixer")]
        [SerializeField] private AudioMixer myAudioMixer;

        [Header("Toggle Interactivity")]
        [SerializeField] private Sprite spriteOn;
        [SerializeField] private Sprite spriteOff;

        [SerializeField] private Image toggleMusicImage;
        [SerializeField] private TextMeshProUGUI toggleMusicText;
        [SerializeField] private Image toggleSoundFXImage;
        [SerializeField] private TextMeshProUGUI toggleSoundFXText;
        [SerializeField] private Image toggleFullscreenImage;
        [SerializeField] private TextMeshProUGUI toggleFullscreenText;

        private Color normalColorButtonPanel;
        private Color selectedColorButtonPanel = Color.white;

        private float onPositionX = 8;
        private float offPositionX = -8;

        private Resolution[] availableResolutions;
        private int currentResolutionIndex = 0;

        // Gestión opciones de vídeo

        // Mostrar controles en UI
        // => Info teclado + ratón / gamepad
        // => if (mobile)=> info controles táctiles
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

            // Video
            InitializeResolutionOptions();
            InitializeQualityOptions();
            fullscreenFXToggle.onValueChanged.AddListener(SetFullscreen); // Add List
        }

        #region Music Options
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

        #endregion

        private void InitializeResolutionOptions()
        {
            availableResolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();
            List<string> options = new List<string>();
            for (int i = 0; i < availableResolutions.Length; i++)
            {
                string option = $"{availableResolutions[i].width} x {availableResolutions[i].height}";

                options.Add(option);
                if (availableResolutions[i].width == Screen.currentResolution.width &&
                    availableResolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            options.Add("Auto Detect");

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();

            resolutionDropdown.onValueChanged.AddListener(SetResolution);
        }

        private void InitializeQualityOptions()
        {
            qualityDropdown.ClearOptions();

            List<string> options = new List<string>() { "Low", "Medium", "High" };

            qualityDropdown.AddOptions(options);
            qualityDropdown.value = QualitySettings.GetQualityLevel();
            qualityDropdown.onValueChanged.AddListener(SetQuality);
        }

        public void SetResolution(int index)
        {
            if (index < availableResolutions.Length)
            {
                Resolution resolution = availableResolutions[index];
                Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

                //Debug.Log($"Resolution changed to {resolution.width} x {resolution.height}"); // D
            }
            else
            {
                Resolution resolution = Screen.currentResolution;

                Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
                resolutionDropdown.value = currentResolutionIndex; // Reset drop

                //Debug.Log($"Resolution autodetect changed to {resolution.width} x {resolution.height}"); // D
            }
        }

        public void SetQuality(int index)
        {
            QualitySettings.SetQualityLevel(index); // Set Quality Sett

            Debug.Log($"Quality changed to {QualitySettings.GetQualityLevel()}"); // D
        }

        public void SetFullscreen(bool isFullscrren)
        {
            Screen.fullScreen = isFullscrren;
            if (isFullscrren)
            {
                //myAudioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolumeSlider.value) * 20);
                toggleFullscreenImage.sprite = spriteOn;
                toggleFullscreenText.text = "ON";
                MoveToggleImage(onPositionX, toggleFullscreenImage);
            }
            else
            {
                //myAudioMixer.SetFloat("MusicVolume", -80f);
                toggleFullscreenImage.sprite = spriteOff;
                toggleFullscreenText.text = "OFF";
                MoveToggleImage(offPositionX, toggleFullscreenImage);
            }

            //Debug.Log($"Fullscreen changed to {isFullscrren}");
        }

        #region UI Options
        public void ChangeColorTextButtonPanel(TextMeshProUGUI clickedText)
        {
            textButtonPanelAudio.color = normalColorButtonPanel;
            textButtonPanelVideo.color = normalColorButtonPanel;
            textButtonPanelControls.color = normalColorButtonPanel;

            clickedText.color = selectedColorButtonPanel;
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
        #endregion

        #region Save/Load Options
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
        #endregion        
    }
}
