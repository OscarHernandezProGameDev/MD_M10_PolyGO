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
        [SerializeField] private AudioMixer audioMixer; // Referencia al AudioMixer para manejar volúmenes.

        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource; // Fuente de audio para la música.
        [SerializeField] private AudioSource soundFXSource; // Fuente de audio para efectos de sonido.

        [Header("Audio Clips")]
        public AudioClip menuMusic; // Música del menú principal.
        public AudioClip gameMusic; // Música del juego.
        public AudioClip attackSlash; // Efecto de sonido para ataques.
        public AudioClip attackSlashWithDelay; // Efecto de sonido para ataques con un delay para los enemies
        public AudioClip playerFall; //Efecto para caída de player
        public AudioClip moveSound; // Efecto de sonido para movimiento.
        public AudioClip enemyDeathUpSound; // Sonido de muerte de enemigo al subir.
        public AudioClip enemyDeathDownSound; // Sonido de muerte de enemigo al bajar.

        [Header("Default Volumes")]
        [Range(0f, 1f)] public float defaultGeneralVolume = 1f; // Volumen general por defecto.
        [Range(0f, 1f)] public float defaultMusicVolume = 1f; // Volumen de la música por defecto.
        [Range(0f, 1f)] public float defaultSFXVolume = 1f; // Volumen de efectos por defecto.

        private const string GeneralVolumeKey = "GeneralVolume"; // Clave para guardar el volumen general.
        private const string MusicVolumeKey = "MusicVolume"; // Clave para guardar el volumen de música.
        private const string SFXVolumeKey = "SFXVolume"; // Clave para guardar el volumen de efectos.
        private const string MusicMuteKey = "MusicMute"; // Clave para guardar el estado de mute de la música.
        private const string SFXMuteKey = "SFXMute"; // Clave para guardar el estado de mute de efectos.

        private void Awake()
        {
            // Implementación del patrón Singleton.
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
            // Carga volúmenes desde PlayerPrefs y los aplica al AudioMixer.
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
            // Evita reiniciar la música si ya está sonando.
            if (musicSource.clip == clip) return;

            musicSource.clip = clip; // Asigna el clip.
            musicSource.loop = true; // Loops habilitado para música.
            musicSource.Play(); // Reproduce la música.
        }

        
        public void StopMusic() // Método para detener la música
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
            // Ajusta el volumen de la música, si no está muteada.
            if (IsMusicMuted()) return;

            float adjustedVolume = volume * volume;
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(adjustedVolume) * 20);
        }

        public void SetSFXVolume(float volume)
        {
            // Ajusta el volumen de efectos, si no están muteados.
            if (IsSFXMuted()) return;

            float adjustedVolume = volume * volume;
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(adjustedVolume) * 20);
        }

        public void ToggleMusicMute(bool isMuted)
        {
            // Mutea o desmutea la música.
            audioMixer.SetFloat("MusicVolume", isMuted ? -80f : Mathf.Log10(PlayerPrefs.GetFloat(MusicVolumeKey, defaultMusicVolume)) * 20);
        }

        public void ToggleSFXMute(bool isMuted)
        {
            // Mutea o desmutea los efectos.
            audioMixer.SetFloat("SFXVolume", isMuted ? -80f : Mathf.Log10(PlayerPrefs.GetFloat(SFXVolumeKey, defaultSFXVolume)) * 20);
        }

        public bool IsMusicMuted()
        {
            return PlayerPrefs.GetInt(MusicMuteKey, 0) == 1; // Verifica si la música está muteada.
        }

        public bool IsSFXMuted()
        {
            return PlayerPrefs.GetInt(SFXMuteKey, 0) == 1; // Verifica si los efectos están muteados.
        }
    }
}
