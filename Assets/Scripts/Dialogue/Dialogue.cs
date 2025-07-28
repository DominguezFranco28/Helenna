using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Dialogue : MonoBehaviour
{
    private bool collisionWithPlayer;
    private bool dialogueStarted;
    private int lineIndex;
    private AirPuzzle airPuzzle;
    [SerializeField ]private float typingSpeed = 0.1f;   
    [SerializeField]private OldPlayerBehaviour playerBehaviour;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField, TextArea(3, 5)] private string[] dialogueLines;


    private void Start()
    {
        // Inicializar las variables de estado necesarias
        collisionWithPlayer = false;
        dialogueStarted = false;
        airPuzzle = GetComponent<AirPuzzle>();
}

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"dialogueText.text={dialogueText.text}");
        if (collisionWithPlayer && Input.GetButtonDown("Fire1"))
        {

            //if (lineIndex < dialogueLines.Length)
            //{
            // Acá se abre todo
            if (airPuzzle.CurrentCount < 1)
                return;
            switch (airPuzzle.CurrentCount)
            {
                //ajustar bien esto

                case 3:
                    StartDialogue();
                    GameStateManager.Instance.SetState(GameState.Playing);

                   // dialoguePanel.SetActive(false);
                   
                    //gugar con el gamestate aca arreglar lo del set active, el resot funciona
                    break;
                case 2:
                    StartDialogue();
                    GameStateManager.Instance.SetState(GameState.Playing);

                 //   dialoguePanel.SetActive(false);

                    break;
                case 1:
                    StartDialogue();
             
                    break;
                case 0:
                    StartDialogue();
                
                    break;
                default:
                    StopAllCoroutines();
                    dialogueText.text = dialogueLines[lineIndex];
                    lineIndex = 0;
                    dialogueStarted = false;
                    break;
            }

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("OldPlayer"))
        {
            collisionWithPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("OldPlayer"))
        {
            collisionWithPlayer = false;
        }
    }

    // Se inicia todo
    private void StartDialogue()
    {
        dialogueStarted = true;
        dialoguePanel.SetActive(true);
        StartCoroutine(ShowLine());

    }
            



    private IEnumerator ShowLine()
    {
        dialogueText.text = "";
        foreach (char ch in dialogueLines[lineIndex])
        {
            dialogueText.text += ch;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

}
