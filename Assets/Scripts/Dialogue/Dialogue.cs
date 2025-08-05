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

    [Header("Diálogo único, sin relación con el puzzle")]
    [SerializeField, TextArea(2, 5)] private string oneShotLine;
    private bool collisionWithPlayer;
    private bool dialogueStarted;
    private int lineIndex = 0;
    private bool oneShotDialogueShown = false;
    private Coroutine currentCoroutine;
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
        if (!collisionWithPlayer)
            return;
        // No permitimos input si un diálogo (de cualquier tipo) está en curso
        if (dialogueStarted)
            return;
        // Si no hay puzzle asociado, no debe intentar usar lineindex ni dialgoos multiples.
        if (airPuzzle == null)
            return;

        if (Input.GetKeyDown(KeyCode.E) && airPuzzle !=null)
        {

            int puzzleIndex = airPuzzle.CurrentCount;

            if (puzzleIndex >= 0 && puzzleIndex < dialogueLines.Length)
            {
               //cerrar si ya se esta mostrando el texto completo, para eso el activeself
                if (dialoguePanel.activeSelf && dialogueText.text == dialogueLines[puzzleIndex])
                {
                    EndDialogue();
                }
                // si se esta tipeando el dialogo, omite la animaicon al volver a detectar el input
                else if (dialogueStarted)
                {
                    if (currentCoroutine != null)
                        StopCoroutine(currentCoroutine);

                    dialogueText.text = dialogueLines[puzzleIndex];
                    dialogueStarted = false; 
                }
                // Si no esta tipeando ni iniciado, iniciar dialogo
                else
                {
                    StartDialogue(puzzleIndex);
                }
            }
        }
    

}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("OldPlayer"))
        {
            _dialogBubbleUI.SetActive(true);
            collisionWithPlayer = true;
            //mostrar el oneshot si aplica (no tenga ningun puzzle asignado)
            if (!oneShotDialogueShown && airPuzzle == null && !dialogueStarted)
            {
                ShowOneShotDialogue(oneShotLine);
            }

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
        currentCoroutine = StartCoroutine(ShowLine(index));
        
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
        foreach (char ch in dialogueLines[index])
        {
            dialogueText.text += ch;
            yield return new WaitForSeconds(typingSpeed);
        }
        dialogueStarted = false; // Permite cerrar con proxima E
    }

    //Metodos para mostrar solo un dialogo, llamado a modo de trigger desde otro script.
    public void ShowOneShotDialogue(string line)
    {
        if (oneShotDialogueShown) return;

        oneShotDialogueShown = true;
        dialogueStarted = true;
        dialoguePanel.SetActive(true);
        StartCoroutine(ShowSingleLine(line));
    }

    private IEnumerator ShowSingleLine(string line)
    {
        dialogueText.text = "";
        foreach (char ch in line)
        {
            dialogueText.text += ch;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(3f); // timer para que espere un poco antes de cerrarse solo, no quiero que dependa de imputs
        EndDialogue();
    }
}
