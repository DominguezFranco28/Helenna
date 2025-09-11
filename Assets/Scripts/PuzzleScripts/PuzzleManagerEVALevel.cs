using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManagerEVALevel : MonoBehaviour
{
    public LevelTimer timer;
    public ClosingGate gate;
    
    public Bridge bridgeEast;
    public Bridge bridgeNorth;
    public MovablePlatform elevatorWest;
    public MovablePlatform elevatorEast;
    public ActionLever lever;

    public List<PressurePlate> pressurePlates;

    private int padsPressed = 0;

    //Agregar Semaforos

    private void Start()
    {
        if(gate && timer)
            gate.moveDuration = timer.GetInitialTimeSeconds();
    }

    private void OnEnable()
    {
        if (pressurePlates.Count > 0)
        {
            foreach(PressurePlate pressurePlate in pressurePlates)
            {
                pressurePlate.OnPadPressed += HandlePadPressed;
                pressurePlate.OnPadReleased += HandlePadReleased;
                lever.OnLeverActioned += HandleLever;
            }

        }
    }

    private void OnDisable()
    {
        if (pressurePlates.Count > 0)
        {
            foreach (PressurePlate pressurePlate in pressurePlates)
            {
                pressurePlate.OnPadPressed -= HandlePadPressed;
                pressurePlate.OnPadReleased -= HandlePadReleased;
                lever.OnLeverActioned -= HandleLever;
            }
                
        }
    }

    private void Update()
    {
        if(padsPressed >= 3)
        {
            bridgeNorth.BridgeOpen();
        }
    }

    private void HandlePadPressed(int manualID)
    {
        if (manualID < 3)
        {
            padsPressed += 1;
        }
        else if (manualID == 3)
        {

        }
        else
        {
            Debug.Log("Unconfigured Pressure Pad Pressed");
        }
    }
    private void HandlePadReleased(int manualID)
    {
        if (manualID < 3)
        {
            padsPressed -= 1;
        }
        else if (manualID == 3)
        {

        }
        else
        {
            Debug.Log("Unconfigured Pressure Pad Released");
        }
    }

    private void HandleLever(int manualID)
    {
        bridgeEast.BridgeOpen();
    }
}
