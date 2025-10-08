using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace PolyGo
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; } // Singleton para acceso global.

        [Header("AudioMixer")]
        [SerializeField] private AudioMixer audioMixer; // Referencia al AudioMixer para manejar vol�menes.

        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource; // Fuente de audio para la m�sica.
        [SerializeField] private AudioSource soundFXSource; // Fuente de audio para efectos de sonido.

        [Header("Audio Clips")]
        public AudioClip menuMusic; // M�sica del men� principal.
        public AudioClip gameMusic; // M�sica del juego.
        public AudioClip attackSlash; // Efecto de sonido para ataques.
        public AudioClip attackSlashWithDelay; // Efecto de sonido para ataques con un delay para los enemies
        public AudioClip playerFall; //Efecto para ca�da de player
        public AudioClip moveSound; // Efecto de sonido para movimiento.
        public AudioClip enemyDeathUpSound; // Sonido de muerte de enemigo al subir.
        public AudioClip enemyDeathDownSound; // Sonido de muerte de enemigo al bajar.

        [Header("Default Volumes")]
        [Range(0f, 1f)] public float defaultGeneralVolume = 1f; // Volumen general por defecto.
        [Range(0f, 1f)] public float defaultMusicVolume = 1f; // Volumen de la m�sica por defecto.
        [Range(0f, 1f)] public float defaultSFXVolume = 1f; // Volumen de efectos por defecto.

        private const string GeneralVolumeKey = "GeneralVolume"; // Clave para guardar el volumen general.
        private const string MusicVolumeKey = "MusicVolume"; // Clave para guardar el volumen de m�sica.
        private const string SFXVolumeKey = "SFXVolume"; // Clave para guardar el volumen de efectos.
        private const string MusicMuteKey = "MusicMute"; // Clave para guardar el estado de mute de la m�sica.
        private const string SFXMuteKey = "SFXMute"; // Clave para guardar el estado de mute de efectos.

        private void Awake()
        {
            // Implementaci�n del patr�n Singleton.
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject); // Destruye instancias duplicadas.
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre escenas.

            ApplySavedAudioSettings(); // Aplica los ajustes guardados al inicio.
        }

        public void ApplySavedAudioSettings()
        {
            // Carga vol�menes desde PlayerPrefs y los aplica al AudioMixer.
            float generalVolume = PlayerPrefs.GetFloat(GeneralVolumeKey, defaultGeneralVolume);
            float musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, defaultMusicVolume);
            float sfxVolume = PlayerPrefs.GetFloat(SFXVolumeKey, defaultSFXVolume);

            SetGeneralVolume(generalVolume);
            SetMusicVolume(musicVolume);
            SetSFXVolume(sfxVolume);

            // Aplica los estados de mute desde PlayerPrefs.
            bool isMusicMuted = PlayerPrefs.GetInt(MusicMuteKey, 0) == 1;
            bool isSFXMuted = PlayerPrefs.GetInt(SFXMuteKey, 0) == 1;

            ToggleMusicMute(isMusicMuted);
            ToggleSFXMute(isSFXMuted);
        }

        public void PlayMusic(AudioClip clip)
        {
            // Evita reiniciar la m�sica si ya est� sonando.
            if (musicSource.clip == clip) return;

            musicSource.clip = clip; // Asigna el clip.
            musicSource.loop = true; // Loops habilitado para m�sica.
            musicSource.Play(); // Reproduce la m�sica.
        }

        
        public void StopMusic() // M�todo para detener la m�sica
        {
            musicSource.Stop();
        }

        public void PlaySoundFX(AudioClip clip)
        {
            // Reproduce un efecto de sonido puntual.
            soundFXSource.PlayOneShot(clip);
        }

        public void SetGeneralVolume(float volume)
        {
            // Ajusta el volumen general en el AudioMixer.
            float adjustedVolume = volume * volume; // Curva de volumen exponencial.
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(adjustedVolume) * 20);
        }

        public void SetMusicVolume(float volume)
        {
            // Ajusta el volumen de la m�sica, si no est� muteada.
            if (IsMusicMuted()) return;

            float adjustedVolume = volume * volume;
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(adjustedVolume) * 20);
        }

        public void SetSFXVolume(float volume)
        {
            // Ajusta el volumen de efectos, si no est�n muteados.
            if (IsSFXMuted()) return;

            float adjustedVolume = volume * volume;
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(adjustedVolume) * 20);
        }

        public void ToggleMusicMute(bool isMuted)
        {
            // Mutea o desmutea la m�sica.
            audioMixer.SetFloat("MusicVolume", isMuted ? -80f : Mathf.Log10(PlayerPrefs.GetFloat(MusicVolumeKey, defaultMusicVolume)) * 20);
        }

        public void ToggleSFXMute(bool isMuted)
        {
            // Mutea o desmutea los efectos.
            audioMixer.SetFloat("SFXVolume", isMuted ? -80f : Mathf.Log10(PlayerPrefs.GetFloat(SFXVolumeKey, defaultSFXVolume)) * 20);
        }

        public bool IsMusicMuted()
        {
            return PlayerPrefs.GetInt(MusicMuteKey, 0) == 1; // Verifica si la m�sica est� muteada.
        }

        public bool IsSFXMuted()
        {
            return PlayerPrefs.GetInt(SFXMuteKey, 0) == 1; // Verifica si los efectos est�n muteados.
        }
    }
}
