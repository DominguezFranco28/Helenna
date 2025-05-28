using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmBullet : MonoBehaviour
{
        private PlayerBehaviour _playerMovement;
    private StateMachine _stateMachine;
    private Rigidbody2D _rb;
    public Vector2 _direction;
    private Collider2D _armCol;
    private Collider2D _playerCol;
    [SerializeField] float _shotSpeed;
    
    private TelekinesisForce telekinesisForce;

    // Método para setear la referencia desde afuera, desde el terlekinesis state al instanciar el brazo
    public void SetTelekinesisForce(TelekinesisForce tf)
    {
        telekinesisForce = tf;
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _armCol = GetComponent<Collider2D>();     
        Destroy(gameObject, 2f);
    }
    private void FixedUpdate()
    {
        _rb.velocity = _direction * _shotSpeed;

    }
    public void SetDirection(Vector2 direction)
    {
        _direction = direction.normalized;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("HookPoint"))
        {
            Debug.Log("Impacto con un punto de anclaje!");
            // Avisar al TelekinesisForce que hubo impacto
            if (telekinesisForce != null)
            {
                Vector2 impactPoint = collision.contacts[0].point;
                    float stopDistance = 0.5f;  //Distancia antes del punto 0 de intacto, para que frene un poco antes y no se me solapen las colisiones
                Vector2 stopPoint = impactPoint - _direction.normalized * stopDistance;
                    telekinesisForce.MovePlayerToAnchor(stopPoint); //llamo al metodo pulico del force par aactivar el desplazamiento.Con el parametro stoppoint me aseguro que frene un poquito antes de llegar al anclaje
             Destroy(gameObject);
            }
            // Aplicar mas logica? Minimo la animaicon del gancho
        }

    }
}
