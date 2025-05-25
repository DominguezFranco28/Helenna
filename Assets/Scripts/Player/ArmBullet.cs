using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmBullet : MonoBehaviour
{
    private Rigidbody2D _rb;
    public Vector2 _direction;
    private Collider2D _armCol;
    private Collider2D _playerCol;
    [SerializeField] float _shotSpeed;
    
    private TelekinesisForce telekinesisForce;

    // Método para setear la referencia desde afuera, para poder llamar al metodo del recoil.
    public void SetTelekinesisForce(TelekinesisForce tf)
    {
        telekinesisForce = tf;
    }


    //[SerializeField] int _bulletDamage;
    [SerializeField] GameObject explosion;
    //EnemyHealth enemyHealth;
    // Start is called before the first frame update
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
        if (collision.gameObject.tag == "HookPoint")
        Debug.Log("Impacto con un punto de anclaje!");
        {
            // Avisar al TelekinesisForce que hubo impacto
            if (telekinesisForce != null)
            {
                    telekinesisForce.MouseGetPull(); //llamo al metodo pulico del force par aactivar el desplazamiento. Hay que retocar, afinar y refactorizar.
             Destroy(gameObject);
            }
            // Aplicar mas logica? Minimo la animaicon del gancho
        }

    }
}
