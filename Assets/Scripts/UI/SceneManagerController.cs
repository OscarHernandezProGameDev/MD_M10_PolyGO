using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyGo
{
    public class SceneManagerController : MonoBehaviour
    {
        public static SceneManagerController Instance { get; private set; }
        public string SceneToLoad { get; set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
