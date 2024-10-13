using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyGo.Player
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(GatherInput))]
    public class PlayerManager : TurnManager
    {
        public PlayerController playerController;
        public GatherInput playerInput;

        protected override void Awake()
        {
            base.Awake();
            playerController = GetComponent<PlayerController>();
            playerInput = GetComponent<GatherInput>();
            playerInput.InputEnabled = true;
        }

        void Update()
        {
            if (playerController.IsMoving || gameManager.CurrentTurn != Turn.Player)
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