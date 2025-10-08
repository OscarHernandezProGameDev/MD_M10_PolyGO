using DG.Tweening;
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
        [Header("Apply Changes")]
        [SerializeField] private Button applyButton;
        [SerializeField] private GameObject unSavedChangesPanel;
        [SerializeField] private CanvasGroup unSavedChangesCanvasGroup;

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
        [SerializeField] private TMP_Dropdown shadowQualityDropdown;
        [SerializeField] private Toggle shadowsToggle;
        [SerializeField] private TMP_Dropdown lightingQualityDropdown;
        [SerializeField] private LightingSettings[] lightingSettingsAssets;

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
        [SerializeField] private Image toggleShadowsImage;
        [SerializeField] private TextMeshProUGUI toggleShadowsText;

        public bool UnAppliedChanges { get; private set; }

        private Action onConfirmAction;

        private Color normalColorButtonPanel;
        private Color selectedColorButtonPanel = Color.white;

        private float onPositionX = 8;
        private float offPositionX = -8;

        private Resolution[] availableResolutions;
        private int currentResolutionIndex = 0;

        private ShadowQuality lastShadowQuality = ShadowQuality.All;

        // Mostrar controles en UI
        // => Info teclado + ratón / gamepad
        // => if (mobile)=> info controles táctiles
        private void Start()
        {
            InitializeResolutionOptions();
            InitializeQualityOptions();
            InitializeShadowQualityOptions();
            InitializeLightingQualityOptions();

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
            fullscreenFXToggle.onValueChanged.AddListener(SetFullscreen);
            shadowsToggle.onValueChanged.AddListener(SetShadows);
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

        private void InitializeShadowQualityOptions()
        {
            shadowQualityDropdown.ClearOptions();

            List<string> options = new List<string>() { "Low", "Medium", "High" };

            shadowQualityDropdown.AddOptions(options);
            shadowQualityDropdown.value = (int)QualitySettings.shadows;
            shadowQualityDropdown.onValueChanged.AddListener(SetShadowQuality);
        }

        private void InitializeLightingQualityOptions()
        {
            lightingQualityDropdown.ClearOptions();

            List<string> options = new List<string>() { "Low", "Medium", "High" };

            lightingQualityDropdown.AddOptions(options);
            lightingQualityDropdown.value = 2;
            lightingQualityDropdown.onValueChanged.AddListener(SetLightingQuality);
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

            ActivateApplyButton();
        }

        public void SetQuality(int index)
        {
            QualitySettings.SetQualityLevel(index); // Set Quality Sett

            //Debug.Log($"Quality changed to {QualitySettings.GetQualityLevel()}");

            ActivateApplyButton();
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
            ActivateApplyButton();
        }

        public void SetShadowQuality(int index)
        {
            switch (index)
            {
                case 0:
                    QualitySettings.shadows = ShadowQuality.HardOnly;
                    lastShadowQuality = ShadowQuality.HardOnly;
                    //Debug.Log("Calidad de las sombras establecida en baja");
                    break;
                case 1:
                    QualitySettings.shadows = ShadowQuality.All;
                    QualitySettings.shadowResolution = ShadowResolution.Medium;
                    lastShadowQuality = ShadowQuality.All;
                    //Debug.Log("Calidad de las sombras establecida en media");
                    break;
                case 2:
                    QualitySettings.shadows = ShadowQuality.All;
                    QualitySettings.shadowResolution = ShadowResolution.High;
                    lastShadowQuality = ShadowQuality.All;
                    //Debug.Log("Calidad de las sombras establecida en alta");
                    break;
            }

            ActivateApplyButton();
        }

        public void SetShadows(bool enabledShadow)
        {
            if (enabledShadow)
            {
                QualitySettings.shadows = lastShadowQuality;
                shadowQualityDropdown.interactable = true;

                toggleShadowsImage.sprite = spriteOn;
                toggleShadowsText.text = "ON";
                MoveToggleImage(onPositionX, toggleShadowsImage);

                //Debug.Log($"Sombras habilitadas a : {lastShadowQuality}");
            }
            else
            {
                lastShadowQuality = QualitySettings.shadows;
                QualitySettings.shadows = ShadowQuality.Disable;
                shadowQualityDropdown.interactable = false;

                toggleShadowsImage.sprite = spriteOff;
                toggleShadowsText.text = "OFF";
                MoveToggleImage(offPositionX, toggleShadowsImage);

                //Debug.Log("Sombras desactivadas");
            }

            ActivateApplyButton();
        }

        public void SetLightingQuality(int index)
        {
            if (lightingSettingsAssets == null || lightingSettingsAssets.Length == 0)
            {
                Debug.Log("No hay ningún lighing asset asignado");
                return;
            }

            foreach (var lightingSettings in lightingSettingsAssets)
            {
                if (!lightingSettings)
                    continue;

                switch (index)
                {
                    case 0: //Low Quality
                        lightingSettings.lightmapResolution = 10f;
                        lightingSettings.maxBounces = 0;
                        lightingSettings.lightmapMaxSize = 512;
                        QualitySettings.pixelLightCount = 1;
                        break;
                    case 1: //Medium Quality
                        lightingSettings.lightmapResolution = 20f;
                        lightingSettings.maxBounces = 1;
                        lightingSettings.lightmapMaxSize = 1024;
                        QualitySettings.pixelLightCount = 2;
                        break;
                    case 2: //High Quality
                        lightingSettings.lightmapResolution = 40f;
                        lightingSettings.maxBounces = 2;
                        lightingSettings.lightmapMaxSize = 2048;
                        QualitySettings.pixelLightCount = 4;
                        break;
                }
                //Debug.Log($"Lighting quality set to {index} para {lightingSettings.name}");
            }

            ActivateApplyButton();
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
            {
                applyButton.gameObject.SetActive(true);
                UnAppliedChanges = true;
            }
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

            PlayerPrefs.SetInt("MusicMute", musicToggle.isOn ? 1 : 0);
            PlayerPrefs.SetInt("SFXMute", soundFXToggle.isOn ? 1 : 0);

            //Video data a guardar
            PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
            PlayerPrefs.SetInt("QualityIndex", qualityDropdown.value);
            PlayerPrefs.SetInt("FullscreenOn", fullscreenFXToggle.isOn ? 1 : 0);
            PlayerPrefs.SetInt("ShadowsOn", shadowsToggle.isOn ? 1 : 0);
            PlayerPrefs.SetInt("ShadowQualityIndex", shadowQualityDropdown.value);
            PlayerPrefs.SetInt("LightingQualityIndex", lightingQualityDropdown.value);

            PlayerPrefs.Save();
            Debug.Log("Settings saved!!!");

            applyButton.gameObject.SetActive(false);
            UnAppliedChanges = false;
        }

        private void LoadSettings()
        {
            // Obtener los valores guardados del audio
            generalVolumeSlider.value = PlayerPrefs.GetFloat("GeneralVolume", 1f);
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
            soundFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

            musicToggle.isOn = PlayerPrefs.GetInt("MusicMute", 1) == 1;
            soundFXToggle.isOn = PlayerPrefs.GetInt("SFXMute", 1) == 1;

            // Aplicar los valores cargados del audiop
            SetGeneralVolume(generalVolumeSlider.value);
            SetMusicVolume(musicVolumeSlider.value);
            SetSFXVolume(soundFXSlider.value);

            ToggleMusicOn(musicToggle.isOn);
            ToggleSFXOn(soundFXToggle.isOn);

            // Obtener los valores guardados del video y los aplicamos
            int resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
            resolutionDropdown.value = resolutionIndex;
            SetResolution(resolutionIndex);

            int qualityIndex = PlayerPrefs.GetInt("QualityIndex", QualitySettings.GetQualityLevel());
            qualityDropdown.value = qualityIndex;
            SetQuality(qualityIndex);

            bool isFullscreen = PlayerPrefs.GetInt("FullscreenOn", Screen.fullScreen ? 1 : 0) == 1;
            fullscreenFXToggle.isOn = isFullscreen;
            SetFullscreen(isFullscreen);

            bool shadowsEnabled = PlayerPrefs.GetInt("ShadowsOn", QualitySettings.shadows != ShadowQuality.Disable ? 1 : 0) == 1;
            shadowsToggle.isOn = shadowsEnabled;
            SetShadows(shadowsEnabled);

            int shadowQualityIndex = PlayerPrefs.GetInt("ShadowQualityIndex", (int)QualitySettings.shadows);
            shadowQualityDropdown.value = shadowQualityIndex;
            SetShadowQuality(shadowQualityIndex);

            int lightingQualityIndex = PlayerPrefs.GetInt("LightingQualityIndex", 2);
            lightingQualityDropdown.value = lightingQualityIndex;
            SetLightingQuality(lightingQualityIndex);

            Debug.Log("Settings loaded!!!");

            applyButton.gameObject.SetActive(false);
            UnAppliedChanges = false;
        }

        public void ShowUnsavedChangesPanel(Action confirmAction)
        {
            onConfirmAction = confirmAction;

            if (unSavedChangesPanel)
            {
                unSavedChangesPanel.SetActive(true);

                if (unSavedChangesCanvasGroup)
                {
                    unSavedChangesCanvasGroup.alpha = 0f;
                    unSavedChangesCanvasGroup.DOFade(1f, 0.2f);
                }
            }
        }

        public void HideUnsavedChangesPanel()
        {
            if (unSavedChangesPanel)
            {
                if (unSavedChangesCanvasGroup)
                {
                    unSavedChangesCanvasGroup.DOFade(0f, 0.2f).OnComplete(() => unSavedChangesPanel.SetActive(false));
                }
                else
                    unSavedChangesPanel.SetActive(false);
            }

        }

        public void ConfirmSaveAndProceed()
        {
            SaveAllSettings();
            ProceedWithAction();
        }

        public void ConfirmDiscardAndProceed()
        {
            LoadSettings();
            ProceedWithAction();
        }

        public void CancelUnsavedChanges()
        {
            HideUnsavedChangesPanel();
        }

        private void ProceedWithAction()
        {
            HideUnsavedChangesPanel();
            if (onConfirmAction != null)
            {
                onConfirmAction.Invoke();
                onConfirmAction = null;
            }
            else
                Debug.LogWarning("onConfirmAction es nulo. No hay acción para ejecutar");
        }
        #endregion
    }
}
