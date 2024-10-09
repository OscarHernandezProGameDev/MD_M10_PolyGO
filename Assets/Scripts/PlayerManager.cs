using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyGo.Player
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(GatherInput))]
    public class PlayerManager : MonoBehaviour
    {
        public PlayerController playerController;
        public GatherInput playerInput;

        void Awake()
        {
            playerController = GetComponent<PlayerController>();
            playerInput = GetComponent<GatherInput>();
            playerInput.InputEnabled = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (playerController.IsMoving)
                return;

            playerInput.GetKeyInput();

            if (playerInput.V == 0)
            {
                if (playerInput.H > 0)
                    playerController.MoveRight();
                else if (playerInput.H < 0)
                    playerController.MoveLeft();
            }
            else if (playerInput.H == 0)
            {
                if (playerInput.V > 0)
                    playerController.MoveForward();
                else if (playerInput.V < 0)
                    playerController.MoveBackward();
            }
        }
    }
}