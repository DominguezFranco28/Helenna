using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlatform : PlayerDetector
{
    private bool _playerOnPlatform = false;
    private bool _isActive = false;
    private Collider2D _player;
    private OldPlayerBehaviour _oldPlayerBehaviour;

    public bool canBeUsed = true;

    private void OnInteract()
    {
        if (canBeUsed)
        {
            if (_playerOnPlatform)
            {
                Effect(_player);
            }
        }
        
    }

    private void OnEnable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.InteractPressed += OnInteract;
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.InteractPressed -= OnInteract;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("OldPlayer")) //agregado de logica adicional a la base
        {
            _playerOnPlatform = true;
            _player = collision;
            _oldPlayerBehaviour = _player.GetComponent<OldPlayerBehaviour>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("OldPlayer"))
        {
            _playerOnPlatform = false;
            _player = null;
        }
    }


    public override void Effect(Collider2D collision)
    {
        if (_isActive)
            return;
        TransitionManager.Instance.PlayBlackScreen();
        _oldPlayerBehaviour.SetMovementEnabled(false);
        _oldPlayerBehaviour.StopMovement();
        _isActive = true;
        StartCoroutine(Teleport()); //corrutina para que no se vea el tp insta
       

    }
    public IEnumerator Teleport()
    {
        yield return new WaitForSeconds(1.5f);
        TransitionManager.Instance.FadeIn();
        CharacterManager.Instance.TeleportAllToCurrent();
        _oldPlayerBehaviour.SetMovementEnabled(true);
        _isActive = false;
    }

}