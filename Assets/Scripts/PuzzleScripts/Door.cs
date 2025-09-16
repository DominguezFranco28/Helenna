using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Collider2D barrier;
    private Animator animator;

    public bool door = false;
    public bool toggleDoor = false;

    void Start()
    {
        barrier = GetComponentInChildren<Collider2D>();
        animator = GetComponent<Animator>();

        if (door)
            DoorOpen();
        else
            DoorClose();
    }

    public void DoorOpen()
    {
        door = true;
        if(barrier)
            barrier.enabled = false;
        if (animator) 
            animator.SetBool("IsOpen", true);
    }

    public void DoorClose()
    {
        door = false;
        if (barrier)
            barrier.enabled = true;
        if (animator)
            animator.SetBool("IsOpen", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (toggleDoor)
        {
            toggleDoor = false;
            door = !door;

            if (door)
                DoorOpen();
            else
                DoorClose();
        }

    }
}
