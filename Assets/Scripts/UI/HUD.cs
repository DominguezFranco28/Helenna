using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public bool isEnabled = true;
    private List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    private TextMeshProUGUI characterName;
    
    [SerializeField] private Image Thumbnail;
    [SerializeField] private List<Sprite> thumbnails = new List<Sprite>();

    private void Start()
    {
        SpriteRenderer[] s = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer sprite in s)
            sprites.Add(sprite);
        characterName = GetComponentInChildren<TextMeshProUGUI>();

        SetCharacterThumbnail("nina");
    }

    private void Update()
    {
        
    }

    public void ToggleHUD()
    {
        isEnabled = !isEnabled;
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

        Thumbnail.sprite = thumbnails[speakerId];
    }

}
