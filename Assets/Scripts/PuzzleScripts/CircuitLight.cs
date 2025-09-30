using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitLight : MonoBehaviour
{
    private SpriteRenderer sprite;
    [SerializeField] private bool isOn = false;
    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;
    public int manualID = 0;

    private void Awake()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        if(!sprite)
            Debug.LogError("Sprite not found for light - manualID:" + manualID);
    }

    private void Start()
    {
        if (isOn) TurnOn();
        else TurnOff();
    }

    public void TurnOn()
    {
        if (sprite)
            sprite.color = onColor;
    }
    public void TurnOff()
    {
        if(sprite)
            sprite.color = offColor;
    }
    public void Toggle()
    {
        if (isOn) TurnOff();
        else TurnOn();

        isOn = !isOn;
    }
}
