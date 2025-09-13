using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManagerEVALevel : MonoBehaviour
{
    public VictoryTrigger victory;
    public LevelTimer timer;
    public ClosingGate gate;
    public TMP_Text endScreen;
    public Bridge bridgeEast;
    public Bridge bridgeNorth;
    public MultiElevatorCircuit elevators;
    public ActionLever lever;

    public List<PressurePlate> pressurePlates;

    private int padsPressed = 0;

    public List<GameObject> padLightPairs = new List<GameObject>();

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
            }
        }
        lever.OnLeverActioned += HandleLever;
        timer.OnTimerFinished += GameOver;
        victory.VictoryReached += Victory;
    }

    private void OnDisable()
    {
        if (pressurePlates.Count > 0)
        {
            foreach (PressurePlate pressurePlate in pressurePlates)
            {
                pressurePlate.OnPadPressed -= HandlePadPressed;
                pressurePlate.OnPadReleased -= HandlePadReleased;
            }
        }
        lever.OnLeverActioned -= HandleLever;
        timer.OnTimerFinished -= GameOver;
        victory.VictoryReached -= Victory;
    }

    private void Update()
    {
        if(padsPressed >= 3)
        {
            if(!bridgeNorth.bridge)
                bridgeNorth.BridgeOpen();
        }
        else
        {
            if (bridgeNorth.bridge)
                bridgeNorth.BridgeClose();
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
            elevators.TiggerAllElevators();
        }
        else
        {
            Debug.Log("Unconfigured Pressure Pad Pressed");
        }

        if (padLightPairs.Count > 0)
        {
            if (manualID >= 0 && manualID < 3)
            {
                CircuitLight[] lights = padLightPairs[manualID].GetComponentsInChildren<CircuitLight>();
                foreach (CircuitLight light in lights) light.TurnOn();
            }
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
            elevators.TiggerAllElevators();
        }
        else
        {
            Debug.Log("Unconfigured Pressure Pad Released");
        }

        if (padLightPairs.Count > 0)
        {
            if (manualID >= 0 && manualID < 3)
            {
                CircuitLight[] lights = padLightPairs[manualID].GetComponentsInChildren<CircuitLight>();
                foreach (CircuitLight light in lights) light.TurnOff();
            }
        }

    }

    private void HandleLever(int manualID)
    {
        bridgeEast.BridgeOpen();
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER");
        if (endScreen)
        {
            endScreen.text = "TIME'S UP!\nGAME OVER";
            endScreen.gameObject.SetActive(true);
            TransitionManager.Instance.LoadNextScene();
        }

    }

    private void Victory()
    {
        Debug.Log("VICTORY");
        timer.PauseTimer();
        if (endScreen)
        {
            endScreen.text = "VICTORY!\nYOU ESCAPED!";
            endScreen.gameObject.SetActive(true);
            TransitionManager.Instance.LoadNextScene();
        }
    }
}
