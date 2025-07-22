using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlatform : PlayerDetector
{
    private bool _playerOnPlatform = false;
    private bool _isActive = false;
    private Collider2D _player;
    private OldPlayerBehaviour _oldPlayerBehaviour;
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

    private void Update()
    {
        if (_playerOnPlatform && Input.GetKeyDown(KeyCode.E))
        {
            Effect(_player);
        }
    }

    // Opcional: puedes dejar este método como está, o quitar la llamada en OnTriggerEnter2D si solo quieres el input
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