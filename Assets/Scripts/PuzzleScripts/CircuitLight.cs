using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitLight : MonoBehaviour
{
    private SpriteRenderer sprite;
    [SerializeField] private bool isOn = false;
    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        if (isOn) TurnOn();
        else TurnOff();
    }

    public void TurnOn()
    {
        sprite.color = onColor;
    }
    public void TurnOff()
    {
        sprite.color = offColor;
    }
}
