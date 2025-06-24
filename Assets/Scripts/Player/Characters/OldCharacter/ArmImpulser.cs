
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ArmImpulser : MonoBehaviour
{
    //Variables para ajustar parametros de la fuerza DE IMPULSO
    [SerializeField] private float _recoilDuration;
    [SerializeField] private float _moveSmoothTime;


    //Variable ligadas al Brazo
    [SerializeField] private GameObject _armShot;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private AudioClip _dashSFX;
    [SerializeField] private AudioClip _throwSFX;
    private ArmImpulser _impulser;
    private GameObject _currentArmBullet;
    private Collider2D _playerCol;


    //Variables para obtener Componentes externos al brazo.
    private MousePosition _mousePosition;
    private PlayerBehaviour _movementBehaviour;




    //Metodos publicos para que desde el script del PlayerBehaviour (que gestiona imputs) y el de Armbnullet (que gestiona la logica del brazo lanzado
    //)se acceda a los metodos de esta mecanica principal (privada).

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
        _playerCol = GetComponent<Collider2D>();
        _mousePosition = GetComponent<MousePosition>();  //Dejo establecido el enlace al otro script. De aca puedo recurrir a otros metodos o propiedades.
                                                       //Principalmente, usado para obtener la pos del mouse, que daba problemas cuando lo calculaba en ambos scritps
        _movementBehaviour = GetComponent<PlayerBehaviour>();
        _impulser = this;
        
    }

    private IEnumerator ApplyRecoil(Vector2 anchorPosition, ImpulseType type) //Deberia convertir esto en un estado?
    {
        if (type != ImpulseType.Pull)
            yield break; //Esto aclara que si el impulso no es pull, se corta inmediatamente la corutina.
        {
            SFXManager.Instance.PlaySFX(_dashSFX);          //ejecuta el script del sonido, llama al Singleton.
            _movementBehaviour.SetMovementEnabled(false);
            _movementBehaviour.isRecoiling = true;

            Vector2 velocity = Vector2.zero;
            float smoothTime = _moveSmoothTime; // parámetro configurable como en la caja
            float stopThreshold = 0.5f;

            while (Vector2.Distance(transform.position, anchorPosition) > stopThreshold) //Muevo al jugador mientras exista distancia entre el y el punto de anclaje
            {
                transform.position = Vector2.SmoothDamp(transform.position, anchorPosition, ref velocity, smoothTime);
                yield return null;
            }

            transform.position = anchorPosition; // asegura que termine exactamente en el punto
                                                 //Separo al pj un poco del punto de anclaje porque se bugeaba
            Vector2 directionAway = (transform.position - (Vector3)anchorPosition).normalized;
            float separationDistance = 5; 
            transform.position += (Vector3)(directionAway * separationDistance);

            _movementBehaviour.SetMovementEnabled(true);
            _movementBehaviour.isRecoiling = false;
        }

    }
    private void ThrowArm(ImpulseType type) 
    {
  
        if (_currentArmBullet != null) return; // Si hay un brazo activo, que retorne, solo quiero uno activado a la vez por coherencia.
        SFXManager.Instance.PlaySFX(_throwSFX); 
        Vector2 direction = _mousePosition.MouseWorlPos; //Tomo la prop publica del MousePotition para no duplicar calculo.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotacion del proyectil apra que mire a donde apunta el mouse
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
        GameObject armBullet = GameObject.Instantiate(_armShot, _spawnPoint.position, rotation);
        if (armBullet!= null)
        {
         _currentArmBullet = armBullet; //Guardo la refe del brazo actual
        //Ignorar colisiones para que el brazo no choque con el jugador
        Collider2D bulletCol = armBullet.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(bulletCol, _playerCol);


        var armScript = armBullet.GetComponent<ArmBullet>();
        armScript.SetDirection(direction);
        armScript.SetImpulseForce(_impulser); 
        armScript.SetImpulseType(type);

       
        }
    }
}


