using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyGo.Player
{
    public class PlayerController : MovementController
    {
        private PlayerArrow playerArrow;

        protected override void Awake()
        {
            base.Awake();
            playerArrow = GameObject.Find("PlayerArrow").GetComponent<PlayerArrow>();
        }

        protected override void Start()
        {
            base.Start();
            UpdateGrid();
        }

        protected override void OnBeforeMove()
        {
            base.OnBeforeMove();
            playerArrow.StopArrowsMovement();
        }

        override protected void OnCompleteMove(Vector3 destinationPosition)
        {
            base.OnCompleteMove(destinationPosition);
            UpdateGrid();
            playerArrow.ShowActiveArrows();
        }

        private void UpdateGrid()
        {
            if (gridSystem != null)
                gridSystem.UpdateActivePlayerDot();
        }
    }
}