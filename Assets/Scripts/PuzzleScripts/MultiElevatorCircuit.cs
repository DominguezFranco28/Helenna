using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiElevatorCircuit : MonoBehaviour
{
    public List<MovablePlatform> elevators;

    public void TiggerAllElevators()
    {
        foreach (MovablePlatform elevator in elevators)
            elevator.TriggerElevator();
    }
}
