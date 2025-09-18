using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ButtonPopup : MonoBehaviour
{
    [SerializeField] private InputActionReference actionReference;
    [SerializeField] private GameObject popupSprite;
    private bool showDevice = false;
    private TextMeshProUGUI popupText;

    [SerializeField] private float amplitude = 0.25f;
    [SerializeField] private float frequency = 2f;   
    private Vector3 startPos;

    public string searchOnTag = "player";

    private void Start()
    {
        popupText = GetComponentInChildren<TextMeshProUGUI>();
        if (popupText)
        {
            popupText.text = GetBindingDisplayName();
            
            popupSprite.SetActive(false);
        }
    }

    private void OnEnable()
    {
        InputManager.Instance.ChangeCharacterPressed += CheckIfNear;
    }
    private void OnDisable()
    {
        InputManager.Instance.ChangeCharacterPressed -= CheckIfNear;
    }

    public string GetBindingDisplayName()
    {
        if (actionReference == null || actionReference.action == null)
            return string.Empty;

        var action = actionReference.action;
        if (action.bindings.Count == 0)
            return string.Empty;

        for (int i = 0; i < action.bindings.Count; i++)
        {
            // Ignore composites (like WASD, 2DVector etc.)
            if (action.bindings[i].isComposite || action.bindings[i].isPartOfComposite)
                continue;

            // Get display string
            string display = action.GetBindingDisplayString(i,
                out string deviceLayoutName,
                out string controlPath,
                InputBinding.DisplayStringOptions.DontIncludeInteractions);

            if (!string.IsNullOrEmpty(display))
            {
                if (showDevice && !string.IsNullOrEmpty(deviceLayoutName))
                    return $"{deviceLayoutName}/{display}";
                else
                    return display;
            }
        }

        return string.Empty;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.ToLower().Contains(searchOnTag))
        {
            if (other.GetComponent<IControllable>().GetControl())
            {
                popupSprite.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.ToLower().Contains(searchOnTag))
        {
            if (collision.GetComponent<IControllable>().GetControl())
            {
                popupSprite.SetActive(false);
            }

        }
    }

    private void Update()
    {
        if (popupSprite.activeInHierarchy)
        {
            float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
            popupSprite.transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
        }
        
    }

    private void CheckIfNear()
    {
        popupSprite.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Collider2D>().enabled = true;
    }
}
