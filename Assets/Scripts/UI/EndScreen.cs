using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace PolyGo
{
    public class EndScreen : MonoBehaviour
    {
        [SerializeField] private Volume postProcessVolume;
        [SerializeField] private VolumeProfile blurProfile;
        [SerializeField] private VolumeProfile normalProfile;

        public void EnableCameraBlur(bool state)
        {
            if (postProcessVolume != null && normalProfile != null && blurProfile)
                postProcessVolume.profile = state ? blurProfile : normalProfile;
        }
    }
}
