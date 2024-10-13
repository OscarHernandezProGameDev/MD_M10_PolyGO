using PolyGo.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyGo
{
    public class TurnManager : MonoBehaviour
    {
        private protected GameManager gameManager;
        private protected bool _turnCompleted = false;

        public bool TurnCompleted { get => _turnCompleted; set => _turnCompleted = value; }

        public void TurnFinish()
        {
            _turnCompleted = true;

            gameManager?.UpdateTurn();
        }

        protected virtual void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }
}
