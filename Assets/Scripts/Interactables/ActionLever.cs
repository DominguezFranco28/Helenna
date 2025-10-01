using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLever : MonoBehaviour, IActiveable
{
    private Animator _animator;
    [SerializeField] private AudioClip _SFX;
    public event System.Action<int> OnLeverActioned;
    private CapsuleCollider2D triggerCollider;

    public int manualID;
    public bool canActivate = true;
    public bool startActive = false;
    public bool isActive = false;

    void Start()
    {
        _animator = GetComponent<Animator>();
        triggerCollider = GetComponent<CapsuleCollider2D>();

        if (startActive)
        {
            Activate();
        }
    }

    public void Activate()
    {
        if (canActivate)
        {
            canActivate = false;
            StartCoroutine(ColliderBlip());
            SFXManager.Instance.PlaySFX(_SFX);
            isActive = !isActive;
            UpdateSprite();
            OnLeverActioned?.Invoke(manualID);
        }

    }

    private IEnumerator ColliderBlip()
    {
        triggerCollider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        triggerCollider.enabled = true;
        canActivate = true;
    }

    public void UpdateSprite()
    {
        _animator.SetBool("Activated", isActive);
    }
}