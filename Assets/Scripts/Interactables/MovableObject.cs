using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
[RequireComponent(typeof(BoxCollider2D))]
public class MovableObject : MonoBehaviour, IMovable
{
        [Header("Movimiento suave")]
        public float moveSmoothTime = 0.2f;
        public float stopThreshold = 0.05f;

        [Header("Colisión")]
        public LayerMask obstacleMask; // Capas con las que el objeto debe colisionar

        private Vector2 targetPosition;
        private Vector2 velocity = Vector2.zero;
        private bool isBeingMoved = false;

        public void MoveTo(Vector2 position)
        {
            targetPosition = position;
            isBeingMoved = true;


        }


    void Update()
        {
            if (!isBeingMoved) return;

            Vector2 currentPosition = transform.position;
            Vector2 direction = targetPosition - currentPosition;
            float distance = direction.magnitude;

            // Verifica colisión en la trayectoria
            if (Physics2D.Raycast(currentPosition, direction.normalized, distance, obstacleMask))
            {
                Debug.DrawRay(currentPosition, direction.normalized * distance, Color.red, 0.1f);
                Debug.Log("Movimiento detenido por colisión");
                isBeingMoved = false;
                return;
            }

            // Movimiento suave hacia el destino
            Vector2 newPosition = Vector2.SmoothDamp(currentPosition, targetPosition, ref velocity, moveSmoothTime);
            transform.position = newPosition;

            if (Vector2.Distance(newPosition, targetPosition) < stopThreshold)
            {
                isBeingMoved = false;
                velocity = Vector2.zero;
        }
        Debug.DrawRay(currentPosition, direction.normalized * distance, Color.red, 0.1f);
    }

    }
