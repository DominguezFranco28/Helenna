using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
[RequireComponent(typeof(BoxCollider2D))]
public class MovableObject : MonoBehaviour, IMovable
{
   
    private Rigidbody2D rb;
    private SlidableSurfaceDetector surfaceDetector;
    private BoxCollider2D boxCollider;

    [SerializeField] private LayerMask slidableLayer;
    [SerializeField] private float checkDistanceMultiplier = 1.05f; // Un poco más grande por seguridad

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        surfaceDetector = GetComponent<SlidableSurfaceDetector>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Move(Vector2 direction, int distance)
    {
        if (surfaceDetector != null && surfaceDetector.IsOnSlidableSurface)
        {
            Vector2 movement = direction.normalized * distance;
            Vector2 targetPosition = rb.position + movement;

            // Obtener el tamaño del box collider para el OverlapBox
            Vector2 boxSize = boxCollider.bounds.size * checkDistanceMultiplier;
            Vector2 boxCenter = targetPosition;

        //    // Comprobar si la nueva posición aún toca una superficie deslizable
        //    Collider2D hit = Physics2D.OverlapBox(boxCenter, boxSize, 0f, slidableLayer);

        //    if (hit != null)
        //    {
        //        rb.MovePosition(targetPosition);
        //    }
        //    else
        //    {
        //        Debug.Log("Movimiento cancelado: fuera de zona deslizable");
        //    }
        }
    }

#if UNITY_EDITOR
    // Para visualizar en la escena el área del OverlapBox (debug)
    private void OnDrawGizmosSelected()
    {
        if (boxCollider == null) return;

        Gizmos.color = Color.yellow;
        Vector2 direction = Application.isPlaying ? rb.velocity.normalized : Vector2.right;
        Vector2 futurePosition = Application.isPlaying ? rb.position + direction * 0.5f : (Vector2)transform.position + Vector2.right * 0.5f;
        Vector2 boxSize = boxCollider.bounds.size* checkDistanceMultiplier;

        Gizmos.DrawWireCube(futurePosition, boxSize);
    }
#endif
}
