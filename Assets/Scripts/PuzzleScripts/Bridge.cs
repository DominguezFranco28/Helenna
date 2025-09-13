using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Collider2D barrier;
    private Animator animator;

    public bool bridge = false;
    public bool toggleBridge = false;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        barrier = GetComponentInChildren<Collider2D>();
        animator = GetComponent<Animator>();

        if (bridge)
            BridgeOpen();
        else
            BridgeClose();
    }

    public void BridgeOpen()
    {
        bridge = true;
        if (animator)
            animator.SetBool("Open", true);
        if(barrier)
            barrier.enabled = false;
    }

    public void BridgeClose()
    {
        bridge = false;
        if (barrier)
            barrier.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (toggleBridge)
        {
            toggleBridge = false;
            bridge = !bridge;

            if (bridge)
                BridgeOpen();
            else
                BridgeClose();
        }

    }
}
