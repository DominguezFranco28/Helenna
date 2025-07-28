using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLever : MonoBehaviour , IActiveable
{
    private Animator _animator;
    private Collider2D _collider2D;
    [SerializeField] private PuzzleManager _puzzleManager;
    [SerializeField] private AudioClip _SFX;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();

    }

    public void Activate()
    {
        SFXManager.Instance.PlaySFX(_SFX);
        _puzzleManager.PuzzleCount();
    }

}
