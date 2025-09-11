using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLever : MonoBehaviour , IActiveable
{
    private Animator _animator;
    private Collider2D _collider2D;
    [SerializeField] private AudioClip _SFX;

    public event System.Action<int> OnLeverActioned;
    public int manualID;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();

    }

    public void Activate()
    {
        SFXManager.Instance.PlaySFX(_SFX);
        _animator.SetTrigger("Change");
        OnLeverActioned?.Invoke(manualID);
    }

}
