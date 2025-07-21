using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformDetector : MonoBehaviour
{


    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(_currentPosition, _collider2D.bounds.size, 0, direction.normalized, distance, _obstacleMask);


        if (hit.collider != null)
        {
            float moveDistance = hit.distance - 0.05f; //para que no se pegue a la colision 
            if (moveDistance < 0) moveDistance = 0f;
            Vector2 collisionPoint = _currentPosition + direction.normalized * moveDistance;
            _targetPosition = collisionPoint;
        }
    }
}
