using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyGo
{
    public class AudioTesting : MonoBehaviour
    {
        public AudioClip clip;

        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.playOnAwake = false;
            StartCoroutine(PlaySound());
        }

        private IEnumerator PlaySound()
        {
            yield return new WaitForSeconds(2f);
            audioSource.Play();
        }
    }
}
