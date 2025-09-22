using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlatform : PlayerDetector
{
    private bool _playerOnPlatform = false;
    private bool _isActive = false;
    private Collider2D _player;

    public bool canBeUsed = true;
    public bool singleUse = true;

    private void OnInteract()
    {
        if (canBeUsed)
        {
            if (_playerOnPlatform)
            {
                Effect(_player);
                if (singleUse)
                    canBeUsed = false;
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

        if (collision.tag.ToLower().Contains("player"))
        {
            _playerOnPlatform = true;
            _player = collision;
            //_oldPlayerBehaviour = _player.GetComponent<OldPlayerBehaviour>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.ToLower().Contains("player"))
        {
            _playerOnPlatform = false;
            _player = null;
        }
    }


    public override void Effect(Collider2D collision)
    {
        if (_isActive)
            return;
        if(TransitionManager.Instance)
            TransitionManager.Instance.PlayBlackScreen();

        InputManager.Instance.LockInputs();

        _isActive = true;
        StartCoroutine(Teleport()); //corrutina para que no se vea el tp insta
       

    }
    public IEnumerator Teleport()
    {
        yield return new WaitForSeconds(1.5f);
        if (TransitionManager.Instance)
            TransitionManager.Instance.FadeIn();
        CharacterManager.Instance.TeleportAllToCurrent();

        InputManager.Instance.UnlockInputs();

        _isActive = false;
    }

}