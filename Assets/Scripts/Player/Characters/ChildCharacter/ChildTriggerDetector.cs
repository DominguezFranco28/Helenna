using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ChildTriggerDetector : MonoBehaviour
{
    [SerializeField] private bool _canClimb = false;
    [SerializeField] private bool _canUseZipline = false;
    private Collider2D _climbableCollider;

    private bool _canActivateLever = false;
    private Collider2D _leverCollider;
    public bool CanClimb { get { return _canClimb; } }
    public bool CanUseZipline { get { return _canUseZipline; }set { _canUseZipline = value; } }
    public bool CanActivate { get { return _canActivateLever;} set { _canActivateLever = value; } }
    public Collider2D Climbable { get { return _climbableCollider; } }
    public Collider2D LevelCollider { get { return _leverCollider; } }

    

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Climbable")) 
        {
            _canClimb = true;
            _climbableCollider = collision;
        }
        
        if (collision.CompareTag("Lever"))
        {
            Debug.Log("colisionaste con palanca");
            _canActivateLever = true;
            _leverCollider = collision;
            Debug.Log("en colision");

        }

    }



    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Climbable") || collision.CompareTag("Pushable"))
        {
            _canClimb = false;
            _climbableCollider = null;
        }
        
        if (collision.CompareTag("Lever"))
        {
            Debug.Log("saliste de colision con palanca");
            _canActivateLever = false;
            _leverCollider = null;

        }
        if (collision.CompareTag("Zipline"))
        {
            Debug.Log("saliste de colision con tirolesa");
            _canUseZipline = false;
        }
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Zipline"))

        {
            ArmLineController zipline = collision.gameObject.GetComponent<ArmLineController>();
            if (zipline == null) return;

            Collider2D edge = collision.GetComponent<Collider2D>();
            //clossespoint para obtener el punto del collider mas cercano al jugador
            Vector2 closest = edge.ClosestPoint(transform.position);
            //closest es entonces la pos real mas cercana del collider al jugador
            float distanceToStart = Vector2.Distance(closest, zipline.StartPoint);
            //se mide la distancia entre el punto mas cercano del collider al pj y el punto de inicio de la zipline
            // nos permite saber si el jugador esta suficientemente cerca del inicio para poder usarla

            // Solo si el pj esta dentro de la tolerancia al rededor del starpoint se activa la zipline (cosa de no poder usarla desde el medio)
            //evita que cualquier otra parte de la zipline active la accion
            if (distanceToStart <= 1.5f) // tolerancia agregada para quye no sea un punto tan exacto de colission
            {
                // Solo si el pj esta dentro de la tolerancia al rededor del starpoint se activa la zipline (cosa de no poder usarla desde el medio)
                //evita que cualquier otra parte de la zipline active la accion
                OnUseZipline(); 
            }
        }
    }
    public void OnUseZipline()
    {
        _canUseZipline = true;
    }
    public void ResetZipline()
    {
        _canUseZipline = false;
    }
}
