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
    public PressurePlate pressurePlateSmall;
    public Door doorBig;
    public Door doorSmall;
    public ActionLever lever;
    public List<CircuitLight> lights = new List<CircuitLight>();
    public bool puzzleDoneP1 = false;
    public Bridge bridge;
    
    [Header("Cinematica")]
    public PlayCinematic playCinematic;

    private void CheckAllDone()
    {
        if (playCinematic)
            playCinematic.Play();

        if (puzzleDoneP1)
        {
            StartCoroutine(Victory());
        }
    }

    private IEnumerator Victory()
    {
        Debug.Log("VICTORY");
        yield return new WaitForSeconds(1.5f);
        
        if (endScreen)
        {
            endScreen.text = "Nivel 2 Terminado - Fin de Demo";
            endScreen.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(5f);
            TransitionManager.Instance.ChangeLevel();
        }
    }

    private void OnEnable()
    {
        lever.OnLeverActioned += Puzzle01;
        pressurePlateBig.OnPadPressed += Puzzle01;
        pressurePlateBig.OnPadReleased += Puzzle01;
        pressurePlateSmall.OnPadPressed += Puzzle01;
        pressurePlateSmall.OnPadReleased += Puzzle01;
    }


    private void Puzzle01(int manualID)
    {
        if (!puzzleDoneP1)
        {


            puzzleDoneP1 = true;
            CheckAllDone();
        }
        
    }

}
