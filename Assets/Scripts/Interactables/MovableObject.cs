﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
[RequireComponent(typeof(BoxCollider2D))]
public class MovableObject : MonoBehaviour, IMovable
{
    [Header("Soft Move")]
    [SerializeField] private float _moveSmoothTime = 0.2f;
    [SerializeField] private float _stopThreshold = 0.05f;

    [Header("Collision")]
    [SerializeField] private LayerMask _obstacleMask; // the layers with which the object has to collide
    private Vector2 _targetPosition;
    private Vector2 _velocity = Vector2.zero;
    private bool _isBeingMoved = false;

    private bool _canMove = true;
    public void MoveTo(Vector2 position) // parameter comes from ArmBullet
    {
        _targetPosition = position;
        _isBeingMoved = true;
    }


    void Update()
    {
        if (!_isBeingMoved ||!_canMove) return;

        Vector2 currentPosition = transform.position;
        Vector2 direction = _targetPosition - currentPosition;
        float distance = direction.magnitude;
        //RaycastHit2D floorCheck = Physics2D.Raycast(currentPosition, direction.normalized, distance, LayerMask.GetMask("SlideFloor"));

        //Check for collision on trajectory
        if (Physics2D.Raycast(currentPosition, direction.normalized, distance, _obstacleMask))
        {
            Debug.DrawRay(currentPosition, direction.normalized * distance, Color.red, 0.1f);
            Debug.Log("Movimiento detenido por colisión");
            _isBeingMoved = false;
            return;
        }

        //Soft move to destiny (play w/ inspector in Unity)
        Vector2 newPosition = Vector2.SmoothDamp(currentPosition, _targetPosition, ref _velocity, _moveSmoothTime);
        transform.position = newPosition;

        if (Vector2.Distance(newPosition, _targetPosition) < _stopThreshold)
        {
            _isBeingMoved = false;
            _velocity = Vector2.zero;
        }
        Debug.DrawRay(currentPosition, direction.normalized * distance, Color.red, 0.1f);
    }
    public void StopMove()
   
    {
        transform.position = _targetPosition;
        _canMove = false;
    
    
    }
}
