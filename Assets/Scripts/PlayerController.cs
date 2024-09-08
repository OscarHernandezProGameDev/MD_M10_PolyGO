using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Ease ease;

    [Header("Debug")]
    [SerializeField] private Vector3 destination;
    [SerializeField] private bool isMoving;

    public void Move(Vector3 destinationPosition, float delay = 0.25f)
    {
        isMoving = true;
        destination = destinationPosition;
        transform.DOMove(destinationPosition, moveSpeed).SetDelay(delay).SetEase(ease).OnComplete(() => OnCompleteMove(destinationPosition));
    }

    private void OnCompleteMove(Vector3 destinationPosition)
    {
        transform.position = destinationPosition;
        isMoving = false;
    }

    void Start()
    {
        Move(new Vector3(0, 0, 2), 1);
        Move(new Vector3(-2, 0, 2), 2);
        Move(new Vector3(-2, 0, 4), 3);
        Move(new Vector3(0, 0, 4), 4);
    }
}
