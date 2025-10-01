using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PuzzleManagerLevel01 : MonoBehaviour
{
    public TMP_Text endScreen;
    public DialogueManager dialogueManager;

    [Header("Puzzle 1")]
    public LogicGate notGate;
    public LogicGate andGate0;
    public LogicGate andGate1;
    public List<ActionLever> leversP1 = new List<ActionLever>();
    public List<CircuitLight> lightsP1 = new List<CircuitLight>();
    public bool puzzleDoneP1 = false;
    
    [Header("Puzzle 2")]
    public List<PressurePlate> pressurePlatesP2_A = new List<PressurePlate>();
    public List<Door> gatesA = new List<Door>();
    public List<PressurePlate> pressurePlatesP2_B = new List<PressurePlate>();
    public List<Door> gatesB = new List<Door>();
    public List<CircuitLight> lightsP2 = new List<CircuitLight>();
    public PressurePlate pressurePlateP2_C;
    public Door gateC;
    public bool puzzleDoneP2 = false;
    
    [Header("Puzzle 3")]
    public PressurePlate pressurePlateP3;
    public ActionLever leverP3;
    public List<CircuitLight> lightsP3 = new List<CircuitLight>();
    private bool hasPower = false;
    public bool puzzleDoneP3 = false;


    [Header("Cinematica")]
    public PlayCinematic playCinematic;

    private void CheckAllDone()
    {
        if(puzzleDoneP1 && puzzleDoneP2 && puzzleDoneP3)
        {
            playCinematic.Play();
            Victory();
        }
    }
    private void Victory()
    {
        Debug.Log("VICTORY");
        
        if (endScreen)
        {
            endScreen.text = "VICTORY!\nYOU ESCAPED!";
            endScreen.gameObject.SetActive(true);
            TransitionManager.Instance.LoadNextScene();
        }
    }

    private void OnEnable()
    {
        foreach(ActionLever lever in leversP1)
        {
            lever.OnLeverActioned += Puzzle01;
        }

        foreach (PressurePlate plate in pressurePlatesP2_A)
        {
            plate.OnPadPressed += PadPressedGroupA;
            plate.OnPadReleased += PadPressedGroupA;
        }
        foreach (PressurePlate plate in pressurePlatesP2_B)
        {
            plate.OnPadPressed += PadPressedGroupB;
            plate.OnPadReleased += PadPressedGroupB;
        }
        pressurePlateP2_C.OnPadPressed += PadPressedGroupC;

        leverP3.OnLeverActioned += P3Lever;
        pressurePlateP3.OnPadPressed += P3Pad;
    }


    private void Puzzle01(int manualID)
    {
        if (!puzzleDoneP1)
        {
            List<int> lightsToEnable = new List<int>();
            List<int> lightsToDisable = new List<int>();

            foreach (ActionLever lever in leversP1)
            {
                int id = lever.manualID;
                switch (id)
                {
                    case 1:
                        andGate0.inputA = lever.isActive;
                        break;
                    case 2:
                        andGate0.inputB = lever.isActive;
                        break;
                    case 3:
                        notGate.inputA = lever.isActive;
                        break;
                }
            }

            andGate1.inputA = andGate0.CheckCondition();
            andGate1.inputB = notGate.CheckCondition();

            if (andGate0.CheckCondition())
                lightsToEnable.Add(4);
            else
                lightsToDisable.Add(4);

            if (notGate.CheckCondition())
                lightsToEnable.Add(5);
            else
                lightsToDisable.Add(5);

            if (andGate1.CheckCondition())
                lightsToEnable.Add(6);
            else
                lightsToDisable.Add(6);

            foreach (CircuitLight light in lightsP1)
            {
                if (light.manualID == manualID)
                {
                    light.Toggle();
                }

                if (lightsToEnable.Count > 0)
                    if (lightsToEnable.Contains(light.manualID))
                        light.TurnOn();

                if (lightsToDisable.Count > 0)
                    if (lightsToDisable.Contains(light.manualID))
                        light.TurnOff();

            }

            if (andGate1.CheckCondition())
            {
                foreach (ActionLever lever in leversP1)
                    lever.canActivate = false;

                puzzleDoneP1 = true;
                AdaptiveMusicLayering.Instance.PlayResolutionTone();
                CheckAllDone();
            }
        }
        
    }


    private void P3Lever(int manualID)
    {
        if (!puzzleDoneP3)
        {
            foreach (CircuitLight light in lightsP3)
            {
                if (light.manualID == manualID)
                {
                    light.Toggle();
                }
            }
            hasPower = !hasPower;
        }
        
    }
    private void P3Pad(int manualID)
    {
        if (!puzzleDoneP3)
        {
            if (hasPower)
            {
                foreach (CircuitLight light in lightsP3)
                {
                    if (light.manualID == manualID)
                    {
                        light.Toggle();
                    }
                }
                
                puzzleDoneP3 = true;
                AdaptiveMusicLayering.Instance.PlayResolutionTone();
                CheckAllDone();
            }
        }
        
        
    }

    private void PadPressedGroupA(int manualID)
    {
        foreach (Door door in gatesA)
            door.Toggle();
    }
    private void PadPressedGroupB(int manualID)
    {
        foreach (Door door in gatesB)
            door.Toggle();
    }
    private void PadPressedGroupC(int manualID)
    {
        gateC.Toggle();
        puzzleDoneP2 = true;
        AdaptiveMusicLayering.Instance.PlayResolutionTone();
        CheckAllDone();
        foreach (CircuitLight light in lightsP2)
        {
            if (light.manualID == manualID)
            {
                light.Toggle();
            }
        }
    }

}
