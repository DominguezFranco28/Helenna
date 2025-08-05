using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private GameObject _dialogBubbleUI;
    [SerializeField]private AirPuzzle airPuzzle;
    [SerializeField ]private float typingSpeed = 0.1f;   
    [SerializeField]private OldPlayerBehaviour playerBehaviour;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField, TextArea(3, 5)] private string[] dialogueLines;
    private bool collisionWithPlayer;
    private bool dialogueStarted;
    private int lineIndex = 0;


    private void Start()
    {
        // Inicializar las variables de estado necesarias
        collisionWithPlayer = false;
        dialogueStarted = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.Instance.IsGamePaused())
            return;
        //Debug.Log($"dialogueText.text={dialogueText.text}");
        if (collisionWithPlayer && Input.GetKeyDown(KeyCode.E))
        {
            //if (lineIndex == 0 || lineIndex == 1) //Primero, los dialogos determinados antes de vincularse al contador del puzzle
            //{
            //    if (!dialogueStarted && !dialoguePanel.activeSelf)
            //    {
            //        StartDialogue(lineIndex);
            //        return;
            //    }
            //    else if (dialogueText.text == dialogueLines[lineIndex])
            //    {
            //        // Dentro de la función hay una corrutina, por eso solo se ejecuta si la línea se completó
            //        EndDialogue();
            //        lineIndex++; // Pasar a la siguiente línea fija (0 -> 1)
            //        return;
            //    }
            //    else
            //    {
            //        StopAllCoroutines();
            //        dialogueText.text = dialogueLines[lineIndex];
            //        return;
            //    }
            //} REVISAR ESTO, UN SOLO TEXTO INICIAL QUEDA FEO
            
            
            
            
            lineIndex = airPuzzle.CurrentCount; //seteo con el contador del puzzle, despues de usar el contador para el dialogo default

            if (lineIndex >= 0 && lineIndex < dialogueLines.Length)
            {
                if (!dialogueStarted && !dialoguePanel.activeSelf) //activeSelf par aque un objeto puedar iniciar dialogo siempre, porque par ala palanca si le daba me borraba el dialogo del filtro del aire
                    StartDialogue(lineIndex);


                else if (dialogueText.text == dialogueLines[lineIndex])
                {
                    // Dentro de la función hay una corrutina, por eso solo se ejecuta si la línea se completó
                    EndDialogue();
                }
                else
                {
                    // Acá se llega si se busca "omitir" el completado de la corrutina dentro de NextDialogueLine
                    StopAllCoroutines();
                    dialogueText.text = dialogueLines[lineIndex];

                }
            }
            else
            {
                EndDialogue();//que me lo cierre siempre si ya no hay mas dialogos
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("OldPlayer"))
        {
            _dialogBubbleUI.SetActive(true);
            collisionWithPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("OldPlayer"))
        {
            _dialogBubbleUI.SetActive(false);
            dialoguePanel.SetActive(false);
            collisionWithPlayer = false;
        }
    }

    // Se inicia todo
    private void StartDialogue(int index)
    {
        dialogueStarted = true;
        dialoguePanel.SetActive(true);
        StartCoroutine(ShowLine(index));
        
    }
    private void EndDialogue()
    {
        dialogueStarted = false;
        dialogueText.text = ""; //limpio el texto para poder ussar diferentes textos de otros scripts
        dialoguePanel.SetActive(false);
    }


    private IEnumerator ShowLine(int index)
    {
        dialogueText.text = "";
        foreach (char ch in dialogueLines[lineIndex])
        {
            dialogueText.text += ch;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

}
