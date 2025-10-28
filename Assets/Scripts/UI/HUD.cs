using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public bool isEnabled = true;
    private List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    [SerializeField]private TextMeshProUGUI characterName;
    
    [SerializeField] private Image Thumbnail;
    [SerializeField] private Image Thumbnail2;
    [SerializeField] private Image Thumbnail3;
    [SerializeField] private List<Sprite> thumbnails = new List<Sprite>();

    private CharacterManager characterManager;

    private void Start()
    {
        SpriteRenderer[] s = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer sprite in s)
            sprites.Add(sprite);

      // characterName = GetComponentInChildren<TextMeshProUGUI>();
        characterManager = CharacterManager.Instance;


        ToggleHUD();
    }

    private void OnEnable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.ChangeCharacterPressed += UpdateHUD;
        else
            Debug.LogError("No InputManager found");
    }

    private void UpdateHUD()
    {
        StartCoroutine(DelayedUpdate());
    }

    private IEnumerator DelayedUpdate()
    {
        yield return new WaitForSeconds(0.25f);
        SetCharacterThumbnail(GetCharacter());
    }

    public void ToggleHUD()
    {
        SetCharacterThumbnail(GetCharacter());

        //isEnabled = !isEnabled;
        foreach (SpriteRenderer sprite in sprites)
            sprite.enabled = isEnabled;

        characterName.enabled = isEnabled;
    }

    private void SetCharacterThumbnail(string charName)
    {
        int speakerId = 0; 
        string name = charName.ToLower().Trim();
        characterName.text = charName;

        switch (name)
        {
            case "harold":
                speakerId = 1;
                break;
            case "rex":
                speakerId = 2;
                break;
            case "nina":
                speakerId = 3;
                break;
            default:
                speakerId = 0; //narrator
                break;
        }

        Thumbnail.sprite = thumbnails[speakerId]; //asigna el speaker ID del personaje activo
        int nextId = (speakerId % 3) + 1; // residuo de la divicion del id entre 3 , sumado 1 ajusta el resultado para uqe este dentro de 1 y 3
        int prevId = (speakerId == 1) ? 3 : speakerId - 1;
        // si el ID es 1, el anterior es 3 etc y cicla las imagenes

        Thumbnail2.sprite = thumbnails[prevId];
        Thumbnail3.sprite = thumbnails[nextId]; 
    }

    private string GetCharacter()
    {
        if (characterManager)
        {
            string charName = characterManager.GetActiveCharacter().Trim().ToLower();
            if (charName.Contains("old"))
                return "harold";
            else if (charName.Contains("dog"))
                return "rex";
            else if (charName.Contains("child"))
                return "nina";
            else
                return "";
        }
        return "";
    }
}
