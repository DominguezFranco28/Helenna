using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManagerEVALevel : MonoBehaviour
{
    public VictoryTrigger victory;
    public LevelTimer timer;
    public TMP_Text endScreen;

    [Header("Puzzle 0")]
    public List<PressurePlate> pressurePlatesP0;
    public Door doorP0;
    [Header("Puzzle 1")]
    public PressurePlate pressurePlateP1;
    public Bridge bridgeSouth;
    [Header("Puzzle 2")]
    public PressurePlate pressurePlateP2;
    public Door doorP2;
    [Header("Puzzle 3")]
    public ClosingGate gate;
    public Bridge bridgeEast;
    public Bridge bridgeNorth;
    public MultiElevatorCircuit elevators;
    public ActionLever lever;
    public List<PressurePlate> pressurePlatesP3;
    private int padsPressedP3 = 0;
    private int padsPressedP0 = 0;
    public List<GameObject> padLightPairs = new List<GameObject>();

    private void Start()
    {
        if(gate && timer)
            gate.moveDuration = timer.GetInitialTimeSeconds();
    }

    private void OnEnable()
    {
        if (pressurePlatesP0.Count > 0)
        {
            foreach (PressurePlate pressurePlate in pressurePlatesP0)
            {
                pressurePlate.OnPadPressed += HandlePadPressed;
                pressurePlate.OnPadReleased += HandlePadReleased;
            }
        }

        if (pressurePlateP1)
        {
            pressurePlateP1.OnPadPressed += HandlePadPressed;
            pressurePlateP1.OnPadReleased += HandlePadReleased;
        }
        
        if (pressurePlateP2)
        {
            pressurePlateP2.OnPadPressed += HandlePadPressed;
            pressurePlateP2.OnPadReleased += HandlePadReleased;
        }

        if (pressurePlatesP3.Count > 0)
        {
            foreach(PressurePlate pressurePlate in pressurePlatesP3)
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
        if (pressurePlatesP0.Count > 0)
        {
            foreach (PressurePlate pressurePlate in pressurePlatesP0)
            {
                pressurePlate.OnPadPressed -= HandlePadPressed;
                pressurePlate.OnPadReleased -= HandlePadReleased;
            }
        }

        if (pressurePlateP1)
        {
            pressurePlateP1.OnPadPressed -= HandlePadPressed;
            pressurePlateP1.OnPadReleased -= HandlePadReleased;
        }

        if (pressurePlateP2)
        {
            pressurePlateP2.OnPadPressed -= HandlePadPressed;
            pressurePlateP2.OnPadReleased -= HandlePadReleased;
        }

        if (pressurePlatesP3.Count > 0)
        {
            foreach (PressurePlate pressurePlate in pressurePlatesP3)
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
        if (padsPressedP0 >= 2)
        {
            if (!doorP0.door)
                doorP0.DoorOpen();
        }

        if (padsPressedP3 >= 3)
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
        switch (manualID)
        {
            case 0:
                padsPressedP3 += 1;
                break;
            case 1:
                padsPressedP3 += 1;
                break;
            case 2:
                padsPressedP3 += 1;
                break;
            case 3:
                elevators.TiggerAllElevators();
                break;
            case 4:
                padsPressedP0 += 1;
                break;
            case 5:
                padsPressedP0 += 1;
                break;
            case 6:
                if (!bridgeSouth.bridge)
                    bridgeSouth.BridgeOpen();
                break;
            case 7:
                if (!doorP2.door)
                    doorP2.DoorOpen();
                break;
            default:
                break;
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
        switch (manualID)
        {
            case 0:
                padsPressedP3 -= 1;
                break;
            case 1:
                padsPressedP3 -= 1;
                break;
            case 2:
                padsPressedP3 -= 1;
                break;
            case 3:
                elevators.TiggerAllElevators();
                break;
            case 4:
                padsPressedP0 -= 1;
                break;
            case 5:
                padsPressedP0 -= 1;
                break;
            default:
                break;
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
