using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatform : MonoBehaviour , IMovable
{
    [SerializeField] private GameObject _posA;
    [SerializeField] private GameObject _posB;
    [SerializeField] private GameObject _barrier;
    [SerializeField] private float _moveSmoothTime;
    private GameObject _player;
    private Vector2 _previousPosition;
    private Vector2 _target;
    private Collider2D _collider2D;
    private Rigidbody2D _rb2D;
    private bool _isOnPlatform = false;
    private Vector2 _velocity = Vector2.zero;

    private bool _changePos;
    private bool _activeLever = false;
    public bool ChangePosition { get { return _changePos; } set { _changePos = value; } }
    public bool ActiveLever { get { return _activeLever; } set { _activeLever = value; } }
    void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        _rb2D = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void FixedUpdate()

    {
        if (ActiveLever)
        {
            

            if (ChangePosition)
            {
                _target = _posB.transform.position;
                _barrier.SetActive(true);

            }
           if (!ChangePosition)
            {
                _target = _posA.transform.position;
                _barrier.SetActive(false);
            }
            MoveTo(_target);

        }

    }
    public void MoveTo(Vector2 direction)
    {
        
        Vector2 smoothPos = Vector2.SmoothDamp(_rb2D.position, direction, ref _velocity, _moveSmoothTime);
        _rb2D.MovePosition(smoothPos);

        // si harold esta encima de la paltaforma, lo movemos manual
        if (_player != null && _isOnPlatform)
        {
            Rigidbody2D playerRb = _player.GetComponent<Rigidbody2D>();
            playerRb.MovePosition(smoothPos);
        }

        //esto tuve que agregarlo por la prop que se gestiona desde el FinalPuzzle. Ademas de que se activa la palanca y a su vez cambia la pos. Cuando finaliza el traslado, vuelvo a poner la palanca es false
        if (Vector2.Distance(_rb2D.position, _target) < 0.5f)
        {
            ActiveLever = false;
        }
    }

    private void OnTriggerStay2D (Collider2D collision)
    {
        //harold tiene su propia gestion de fisicas y rb en su script, daba conflictos con un simple transform.
        if (collision.gameObject.CompareTag("OldPlayer"))
        {
            _player = collision.gameObject; // no podia iniciar el script con el oldplayer instanciado porque no se reseteaba la referncia y em la pegaba a la paltaforma
            
            _isOnPlatform = true;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("OldPlayer"))
        {
            _player = null; //limpio el registro del player
           
            _isOnPlatform = false;

        }
    }
}
