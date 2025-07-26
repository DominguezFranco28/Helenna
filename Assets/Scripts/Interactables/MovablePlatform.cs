using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatform : MonoBehaviour , IMovable
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _posA;
    [SerializeField] private GameObject _posB;
    [SerializeField] private GameObject _barriers;
    [SerializeField] private float _moveSmoothTime;
    
    private Collider2D _collider2D;
    private Rigidbody2D _rb2D;
    private Vector2 _velocity = Vector2.zero;
    private bool _changePos = true;
    private Vector2 _target;

    public bool ChangePosition { get { return _changePos; } set { _changePos = value; } }
    void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        _rb2D = GetComponent<Rigidbody2D>();
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //tengo que buscarle la vuelta para refactorizar esto
        if (ChangePosition)
        {
            _target = _posB.transform.position;
            MoveTo(_target);
        }
       if (!ChangePosition)
        {
            _target = _posA.transform.position;
            MoveTo(_target);

        }


    }
    public void MoveTo(Vector2 direction)
    {
        
        Vector2 smoothPos = Vector2.SmoothDamp(_rb2D.position, direction, ref _velocity, _moveSmoothTime);
        _rb2D.MovePosition(smoothPos);
        //if (Vector2.Distance(_rb2D.position, _target) < 0.5f)
        //{
        //    _isInStart = !_isInStart;
        //}
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);//hago al jugador hijo de la plataforma para que pueda moverse
            _barriers.gameObject.SetActive(true);//prendo las barreras de la plataforma para que el jugador pueda moverse.
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }

}
