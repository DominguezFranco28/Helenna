using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [Header("Weight Settings")]
    [SerializeField] private float triggerWeight = 0f;
    [SerializeField] private float totalWeight = 0f;
    [SerializeField] private bool _needHold = false;

    [SerializeField] private AudioClip _audioClip;
    private Collider2D _collider2D;
    private Animator _animator;

    public bool NeedHold { get { return _needHold; } } //prop solo lectura, la uso para detectar en el puzzle que use placa de presion en conjunto.
                                                       //Cuando se resuelve el puzzle, las apaga a todas las relacionadas llamando al metodo publico

    public event System.Action<int> OnPadPressed;
    public event System.Action<int> OnPadReleased;

    private bool isPressed = false;

    public int manualID = 0;

    private void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        WeightedObject weighted = other.GetComponent<WeightedObject>();
        if (weighted)
        {
            totalWeight += weighted.GetWeight();

            if (totalWeight >= triggerWeight)
                ActivatePlate();
        }
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        WeightedObject weighted = other.GetComponent<WeightedObject>();
        if (weighted)
        {
            totalWeight -= weighted.GetWeight();
            if (totalWeight < 0f) totalWeight = 0f;

            if (totalWeight < triggerWeight)
            {
                if (_needHold)
                    DeactivatePlate();
            }
            
        }
            
    }

    private void ActivatePlate()
    {
        isPressed = true;

        Debug.Log("Placa de presion activada");
        _animator.SetBool("IsPressed", true);
        SFXManager.Instance.PlaySFX(_audioClip);

        OnPadPressed?.Invoke(manualID);
    }

    public void DeactivatePlate()
    {
        isPressed = false;

        //Metodo publico para que el puzzle pueda apagar la placa de presion, por ejemplo si se resuelve el puzzle y se quiere apagar todas las placas de presion
        Debug.Log("Placa de presion desactivada");
        _animator.SetBool("IsPressed", false);
        SFXManager.Instance.PlaySFX(_audioClip);

        OnPadReleased?.Invoke(manualID);
    }


}