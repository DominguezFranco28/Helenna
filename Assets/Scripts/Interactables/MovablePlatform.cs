using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatform : MonoBehaviour , IMovable
{
    [SerializeField] private GameObject _posA;
    [SerializeField] private GameObject _posB;

    [SerializeField] private List<GameObject> _activeBarriers;
    [SerializeField] private List<GameObject> _topPosBarriers;
    
    [SerializeField] private float _moveSmoothTime;
    
    private Vector2 _target;
    private Vector2 _velocity = Vector2.zero;
    private bool onGround = true;
    public bool elevatorTriggered = false;
    public bool elevatorMoving = false;
    private float _elapsedTime;
    [SerializeField] private float speed = 5f;       // Movement speed
    [SerializeField] private float smoothTime = 0.1f; // Smoothing factor

    public bool setStartPosTop = false;

    void Start()
    {
        if (setStartPosTop)
        {
            TriggerElevator();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (elevatorTriggered)
        {
            elevatorTriggered = false;
            elevatorMoving = true;
            
            foreach (GameObject barrier in _topPosBarriers)
            {
                barrier.SetActive(false);
            }
            foreach (GameObject barrier in _activeBarriers)
            {
                barrier.SetActive(true);
            }

            if (onGround)
            {
                onGround = false;
                _target = _posB.transform.position;
            }
            else
            {
                onGround = true;
                _target = _posA.transform.position;
            }
        }

        if (elevatorMoving)
        {
            MoveTo(_target);
        }

    }

    public void MoveTo(Vector2 targetPoint)
    {
        if (targetPoint == null) return;
        Vector2 smoothPos = Vector2.SmoothDamp(transform.position,targetPoint,ref _velocity,smoothTime,speed);
        transform.position = smoothPos;
        if (Vector2.Distance(transform.position, targetPoint) <= 0.1f)
        {
            elevatorMoving = false;
            transform.position = targetPoint;

            foreach (GameObject barrier in _activeBarriers)
            {
                barrier.SetActive(false);
            }

            if (!onGround)
            {
                foreach (GameObject barrier in _topPosBarriers)
                {
                    barrier.SetActive(true);
                }
            }          
            
        }
        //_rb2D.MovePosition(smoothPos);

        

        /*// si harold esta encima de la paltaforma, lo movemos manual
        if (_player != null && _isOnPlatform)
        {
            Rigidbody2D playerRb = _player.GetComponent<Rigidbody2D>();
            playerRb.MovePosition(smoothPos);

        }*/
        
        
        /*
        //esto tuve que agregarlo por la prop que se gestiona desde el FinalPuzzle. Ademas de que se activa la palanca y a su vez cambia la pos. Cuando finaliza el traslado, vuelvo a poner la palanca es false
        if (Vector2.Distance(_rb2D.position, _target) < 0.5f)
        {
            elevatorMoving = false;
            foreach (GameObject barrier in _desactiveBarriers)
            {
                barrier.SetActive(false);
            }
            if (!_changePos) //si se queda en a, que me prenda todas las barreras desactivadas
            {
                foreach (GameObject barrier in _desactiveBarriers)
                {
                    barrier.SetActive(true);
                }
                foreach (GameObject barrier in _activeBarriers) // y que me apague todas las barreras que estaban activadas rdurante el dezplazamiento
                {
                    barrier.SetActive(false);
                }
            }
        }*/
    }

    private void OnTriggerStay2D (Collider2D collision)
    {
        //harold tiene su propia gestion de fisicas y rb en su script, daba conflictos con un simple transform.
        /*if (collision.gameObject.CompareTag("OldPlayer"))
        {
            _player = collision.gameObject; // no podia iniciar el script con el oldplayer instanciado porque no se reseteaba la referncia y em la pegaba a la paltaforma
            
            _isOnPlatform = true;
        }*/

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        /*if (collision.gameObject.CompareTag("OldPlayer"))
        {
            _player = null; //limpio el registro del player
           
            _isOnPlatform = false;

        }*/
    }

    public void TriggerElevator()
    {
        elevatorTriggered = true;
    }
}
