using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ArmImpulser : MonoBehaviour
{
    //Variables para ajustar parametros de la fuerza telequinetica
    [SerializeField] private float pushRange;
    [SerializeField] private float recoilDuration;
    [SerializeField] private float moveSmoothTime;
    [SerializeField] private LayerMask pushableLayer;


    //Variable ligadas al Brazo
    [SerializeField] private GameObject _armShot;
    [SerializeField] private Transform _spawnPoint;
    private ArmImpulser _impulser;
    private GameObject _currentArmBullet;
    private Collider2D _armCol;
    private Collider2D _playerCol;

    //Variables ligadas al powerup
    private int originalForce;
    private bool upgradeActive = false;
    private bool upgradeUsed = false; //Bandera para indicar si el jugador uso el powerup   

    //Variables para obtener Componentes.
    private Rigidbody2D playerRB;
    private MousePosition mousePosition;
    private PlayerBehaviour movementBehaviour;
    private PlayerEnergy playerEnergy;
    //[SerializeField] private LineRenderer pushRenderer;

    //Variables ligadas al patron Observer
    private StatsManager statsManager;


    //Metodos publicos para que desde el script del behhavo (que gestiona imputs) y el de armbnullet (que gestiona la logica del brazo lanzado )se acceda a los metodos de esta mecanica principal (privada).

    public void MovePlayerToAnchor(Vector2 anchorPosition, ImpulseType type)
    {
        StartCoroutine(ApplyRecoil(anchorPosition, type));
    }
    public void GetThrowArm(ImpulseType type)
    {
        ThrowArm(type);
    }

    
    void Start()
    {
        statsManager = GetComponent<StatsManager>();
        playerEnergy = GetComponent<PlayerEnergy>(); //La necesito para estar pendiente de la energia del jugador.
        playerRB = GetComponent<Rigidbody2D>();
        _playerCol = GetComponent<Collider2D>();
        mousePosition = GetComponent<MousePosition>();  //Dejo establecido el enlace al otro script. De aca puedo recurrir a otros metodos o propiedades.
                                                        //Principalmente, usado para obtener la pos del mouse, que daba problemas cuando lo calculaba en ambos scritps
        movementBehaviour = GetComponent<PlayerBehaviour>();
        _impulser = this;
        
    }
    private void Update()
    {
        /*ShowRaycastLine(); */ //Para mostrar el Raycast al jugador, tal vez lo quite                           //Spoiler del futuro, no lo saque, lo use para dar fuerza a la mecanica principal.
    }


    private IEnumerator ApplyRecoil(Vector2 anchorPosition, ImpulseType type)
    {
        if (type != ImpulseType.Pull)
            yield break; //Esto aclara que si el impulso no es pull, se corta inmediatamente la corutina.
        {
            movementBehaviour.canMove = false;
            movementBehaviour.isRecoiling = true;

            Vector2 velocity = Vector2.zero;
            float smoothTime = moveSmoothTime; // parámetro configurable como en la caja
            float stopThreshold = 0.05f;

            while (Vector2.Distance(transform.position, anchorPosition) > stopThreshold) //Muevo al jugador mientras exista distancia entre el y el punto de anclaje
            {
                transform.position = Vector2.SmoothDamp(transform.position, anchorPosition, ref velocity, smoothTime);
                yield return null;
            }

            transform.position = anchorPosition; // asegura que termine exactamente en el punto
            movementBehaviour.canMove = true;
            movementBehaviour.isRecoiling = false;
        }

    }
    private void ThrowArm(ImpulseType type) 
    {
  
        if (_currentArmBullet != null) return; // Si hay un brazo activo, que retorne, solo quiero uno activado a la vez por coherencia.

        Vector2 direction = mousePosition.MouseWorlPos; //Tomo la prop publica del MousePotition para no duplicar calculo.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotacion del proyectil apra que mire a donde apunta el mouse
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
        GameObject armBullet = GameObject.Instantiate(_armShot, _spawnPoint.position, rotation);
        if (armBullet!= null)
        {
         _currentArmBullet = armBullet; //Guardo la refe del brazo actual
        //Ignorar colisiones para qeu el brazo no choque con el jugador
        Collider2D bulletCol = armBullet.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(bulletCol, _playerCol);


        var armScript = armBullet.GetComponent<ArmBullet>();
        armScript.SetDirection(direction);
        armScript.SetImpulseForce(_impulser); 
        armScript.SetImpulseType(type);

       
        }
    }
    //private void ShowRaycastLine()
    //{
    //    Vector2 direction = mousePosition.MouseWorlPos;
    //    Vector3 pushStart = _spawnPoint.position;
    //    Vector3 pushEnd = pushStart + (Vector3)direction * pushRange;
    //    //Mismo check del hit con las capas que me interesa de antes, pero ahora solo para modificar color.
    //    RaycastHit2D hit = Physics2D.Raycast(_spawnPoint.position, direction, pushRange, pushableLayer);

    //    if (hit.collider != null)
    //    {
    //        pushRenderer.startColor = new Color(0f, 1f, 1f, 1f);   // Cyan si el hit es Pushable.
    //        pushRenderer.endColor = new Color(0f, 1f, 1f, 0f);
    //        pushRenderer.SetPosition(0, pushStart);
    //        pushRenderer.SetPosition(1, pushEnd);
    //    }
    //    else
    //    {
    //        pushRenderer.startColor = new Color(1f, 0f, 0f, 1f); // Rojo si no es Pushable.
    //        pushRenderer.endColor = new Color(1f, 0f, 0f, 0f);

    //        pushRenderer.SetPosition(0, pushStart);
    //        pushRenderer.SetPosition(1, pushEnd);
    //    } 
    //}
}


