using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyGo
{
    public class LightingRebuilder : MonoBehaviour
    {
        void Start()
        {
            //Forzar la recargar de la iluminaci�n global al iniciar la escena
            DynamicGI.UpdateEnvironment();
        }
    }
}
