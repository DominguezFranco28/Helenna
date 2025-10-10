using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PuzzleManagerLevel02 : MonoBehaviour
{
    public TMP_Text endScreen;
    public DialogueManager dialogueManager;

    [Header("Puzzle 1")]
    public PressurePlate pressurePlateBig;
    public PressurePlate pressurePlateMedium;
    public List<CircuitLight> secondaryDoorLights = new List<CircuitLight>();
    public CircuitLight mediumLight;
    private int padsPressed = 0;

    public PressurePlate pressurePlateSmall;
    public Door doorBig;
    private bool doorIsClosing = false;
    public Door doorSmall;
    public ActionLever lever;

    public List<CircuitLight> puzzleLights = new List<CircuitLight>();
    public List<CircuitLight> doorLights = new List<CircuitLight>();

    public bool puzzleDoneP1 = false;
    public Bridge bridge;

    public float doorCloseTime = 0.3f;

    [Header("Cinematica")]
    public PlayCinematic playCinematic;

    public ParticleSystem p1aSteamVFX;
    public ParticleSystem p1bSteamVFX;
    public AudioClip steamSFX;

    private void OnEnable()
    {
        lever.OnLeverActioned += LeverActioned;

        pressurePlateBig.OnPadPressed += PadPressed;
        pressurePlateBig.OnPadReleased += PadReleased;

        pressurePlateMedium.OnPadPressed += PadPressed;
        pressurePlateMedium.OnPadReleased += PadReleased;

        pressurePlateSmall.OnPadPressed += SmallPad;
    }
    private void OnDisable()
    {
        lever.OnLeverActioned -= LeverActioned;

        pressurePlateBig.OnPadPressed -= PadPressed;
        pressurePlateBig.OnPadReleased -= PadReleased;

        pressurePlateMedium.OnPadPressed -= PadPressed;
        pressurePlateMedium.OnPadReleased -= PadReleased;

        pressurePlateSmall.OnPadPressed -= SmallPad;
    }


    private void PadPressed(int manualID)
    {
        if (!puzzleDoneP1)
        {
            foreach (CircuitLight light in secondaryDoorLights)
            {
                if(light.manualID == manualID)
                    light.TurnOn();
            }   

            padsPressed += 1;
            if (padsPressed > 2 || padsPressed < 0) padsPressed = 0;

            if(padsPressed == 2)
            {
                if (!doorIsClosing)
                {
                    doorBig.DoorOpen();
                    foreach (CircuitLight light in doorLights)
                        light.TurnOn();
                }
                    
            }

        }
        
    }

    private void PadReleased(int manualID)
    {
        if (!puzzleDoneP1)
        {
            foreach (CircuitLight light in secondaryDoorLights)
            {
                if (light.manualID == manualID)
                    light.TurnOff();
            }

            padsPressed -= 1;
            if (padsPressed > 2 || padsPressed < 0) padsPressed = 0;

            if (padsPressed < 2)
            {
                if (!doorIsClosing)
                {
                    doorIsClosing = true;
                    StartCoroutine(CloseDoorTimed());
                }
            }
            
            
        }

    }
    private IEnumerator CloseDoorTimed()
    {
        foreach (CircuitLight light in doorLights)
        {
            yield return new WaitForSeconds(doorCloseTime/ doorLights.Count);
            light.TurnOff();
        }
        doorBig.DoorClose();
        doorIsClosing = false;
    }

    private void SmallPad(int manualID)
    {
        if (!puzzleDoneP1)
            doorSmall.DoorOpen();
    }

    private void LeverActioned(int manualID)
    {
        lever.canActivate = false;

        foreach (CircuitLight light in puzzleLights)
            light.TurnOn();

        puzzleDoneP1 = true;
        StartCoroutine(OpenBridge());
    }

    private IEnumerator OpenBridge()
    {
        if (steamSFX)
            SFXManager.Instance.PlaySFX(steamSFX);
        if(p1aSteamVFX)
            p1aSteamVFX.Play();

        if (playCinematic)
            playCinematic.Play();

        yield return new WaitForSeconds(4.25f);
        if(steamSFX)
            SFXManager.Instance.PlaySFX(steamSFX);
        if(p1bSteamVFX)
            p1bSteamVFX.Play();
        yield return new WaitForSeconds(0.25f);
        bridge.BridgeOpen();

        StartCoroutine(Victory());
    }

    private IEnumerator Victory()
    {
        Debug.Log("VICTORY");
        yield return new WaitForSeconds(1.5f);

        if (endScreen)
        {
            endScreen.text = "Nivel 2 Terminado";
            endScreen.gameObject.SetActive(true);

            yield return new WaitForSeconds(5f);
            TransitionManager.Instance.ChangeLevel();
        }
    }

    private void Start()
    {
        if (playCinematic)
            playCinematic.Play();
    }
}
