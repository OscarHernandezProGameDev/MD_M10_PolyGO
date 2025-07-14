using PolyGo.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyGo
{
    public class CameraController : MonoBehaviour
    {
        public Transform player;    // Referencia al jugador
        public float rotationSpeed = 100f;    // Velocidad de rotación de la cámara
        public float rotationLimit = 45f;   // Límite de rotación de la cámara
        public float returnValue = 1f;      // Tiempo pata esperar antes de volver
        public float smoothSpeed = 2f;     // Velocidad de suavizado al volver

        public float zoomSpeed = 2f;         // Velocidad de zoom
        public float minZoom = 5f;          // Límite mínimo de zoom
        public float maxZoom = 15f;         // Límite máximo de zoom

        private Vector3 initialOffset;      // Desplazamiento inicial desde el jugador
        private float initialDistance;      // Distancia inicial entre la cámara y el jugador

        private float targetZoom;   // Nivel de zoom objetivo
        private float currentZoom;  // Nivel de zoom actual

        private float targetRotation;   // Rotación objetivo de la cámara
        private float currentRotation;   // Rotación actual de la cámara

        private float rotationTimer = 0f;
        private bool isReturning = false;

        private float initialPitch; // Angulo inicial en X (inclinación)
        private float currentPitch; // Angulo actual en X

        private GatherInput gatherInput;

        private void Awake()
        {
            gatherInput = FindAnyObjectByType<GatherInput>();

            // Calcular el offset actual
            initialOffset = transform.position - player.position;
            initialDistance = initialOffset.magnitude;

            // Inicializar los niveles de zoom
            targetZoom = maxZoom / initialDistance;
            currentZoom = targetZoom;

            // Almacenar la rotación inicial en X (inclinación)
            initialPitch = transform.rotation.eulerAngles.x;
            currentPitch = initialPitch;
        }

        private void Update()
        {
            if (gatherInput == null || player == null)
                return;

            HandleZoom();
            HandleRotation();

            // Suavizar el zoom y la rotación
            currentZoom = Mathf.Lerp(currentZoom, targetZoom, smoothSpeed * Time.deltaTime * smoothSpeed);
            currentRotation = Mathf.Lerp(currentRotation, targetRotation, smoothSpeed * Time.deltaTime * smoothSpeed);

            // Calcular el offset actual con zoom y rotación
            Quaternion rotation = Quaternion.Euler(currentPitch, currentRotation, 0f);
            Vector3 zoomedOffset =  new Vector3(0f, 0f, -initialDistance*currentZoom);
            Vector3 rotatedOffset = rotation * zoomedOffset;

            // Actualizar la posición de la cámara
            transform.position = player.position + rotatedOffset;

            // Actualizar la rotación de la cámara sin aplicar el pitch (la inclinación)
            transform.rotation = rotation;

        }

        private void HandleZoom()
        {
            float scrollValue = gatherInput.ZoomValue;

            if (Mathf.Abs(scrollValue) > 0.01f)
            {
                float zoomChange = scrollValue * zoomSpeed * Time.deltaTime;
                targetZoom = Mathf.Clamp(targetZoom - zoomChange, minZoom / initialDistance, maxZoom / initialDistance);
            }
        }

        private void HandleRotation()
        {
            if (gatherInput.IsRotating)
            {
                rotationTimer = 0f;
                isReturning = false;

                // Actualizar el ángulo objetivo en Y basado en la entrada
                float rotationAmout = gatherInput.RotateValue.x * rotationSpeed * Time.deltaTime;
                targetRotation += rotationAmout;
                targetRotation = Mathf.Clamp(targetRotation, -rotationLimit, rotationLimit);
            }
            else
            {
                rotationTimer += Time.deltaTime;
                if (rotationTimer >= returnValue && !isReturning)
                {
                    isReturning = true;
                }

                if (isReturning)
                {
                    // Interpolar el angulo objetivo de regreso a zero
                    targetRotation = Mathf.Lerp(targetRotation, 0, smoothSpeed * Time.deltaTime * smoothSpeed);

                    // Si estamos cerca de zero,fijamos la rotación a cero y detener la rotación
                    if (Mathf.Abs(targetRotation) < 0.1f)
                    {
                        targetRotation = 0f;
                        isReturning = false;
                    }
                }
            }
        }
    }
}
