using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyGo
{
    public class LightingRebuilder : MonoBehaviour
    {
        void Start()
        {
            //Forzar la recargar de la iluminación global al iniciar la escena
            DynamicGI.UpdateEnvironment();
        }
    }
}
