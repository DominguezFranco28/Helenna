using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchScreenControls : MonoBehaviour
{
    public bool onScreenButtonsEnabled = false;
    public GameObject buttons;

    public Button qButton;
    public Button eButton;
    public Button wButton;
    public Button sButton;
    public Button aButton;
    public Button dButton;
    public Button tabButton;
    public Button spaceButton;
    public Button pauseButton;

    void Start()
    {
        if(OnScreenControlsManager.Instance)
            onScreenButtonsEnabled = OnScreenControlsManager.Instance.onScreenButtonsEnabled;

        buttons.SetActive(onScreenButtonsEnabled);

        if (qButton)
            qButton.onClick.AddListener(SpecialAction);
        if (spaceButton)
            spaceButton.onClick.AddListener(Action);
        if (eButton)
            eButton.onClick.AddListener(Interact);
        if (pauseButton)
            pauseButton.onClick.AddListener(Pause);
        if (tabButton)
            tabButton.onClick.AddListener(SwapCharacter);

        // Movement buttons use press + release events instead of onClick
        AddTouchHandlers(wButton, Vector2.up);
        AddTouchHandlers(sButton, Vector2.down);
        AddTouchHandlers(aButton, Vector2.left);
        AddTouchHandlers(dButton, Vector2.right);
    }

    void AddTouchHandlers(Button button, Vector2 dir)
    {
        if (button == null) return;

        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

        // OnPointerDown- move start
        var down = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        down.callback.AddListener((_) => InputManager.Instance?.TriggerMove(dir));
        trigger.triggers.Add(down);

        // OnPointerUp- move stop
        var up = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        up.callback.AddListener((_) => InputManager.Instance?.TriggerMoveCanceled());
        trigger.triggers.Add(up);
    }

    public void EnableOnScreenButtons()
    {
        onScreenButtonsEnabled = true;
        buttons.SetActive(onScreenButtonsEnabled);
    }

    public void DisableOnScreenButtons()
    {
        onScreenButtonsEnabled = false;
        buttons.SetActive(onScreenButtonsEnabled);
    }

    // --- Button callbacks ---
    public void Action()
    {
        if (InputManager.Instance)
        {
            InputManager.Instance.TriggerActionPressed();
            SkipDialogue();
        }
            
    }

    public void SpecialAction()
    {
        if (InputManager.Instance)
        {
            InputManager.Instance.TriggerSpecialActionPressed();
            SkipDialogue();
        }
            
    }

    public void Interact()
    {
        if (InputManager.Instance)
        {
            InputManager.Instance.TriggerInteractPressed();
            SkipDialogue();
        }
            
    }

    public void SwapCharacter()
    {
        if (InputManager.Instance)
            InputManager.Instance.TriggerChangeCharacterPressed();
    }

    public void Pause()
    {
        if (InputManager.Instance)
            InputManager.Instance.TriggerPausePressed();
    }

    public void Forward()
    {
        if (InputManager.Instance)
            InputManager.Instance.TriggerMove(Vector2.up);
    }

    public void Backward()
    {
        if (InputManager.Instance)
            InputManager.Instance.TriggerMove(Vector2.down);
    }

    public void Left()
    {
        if (InputManager.Instance)
            InputManager.Instance.TriggerMove(Vector2.left);
    }

    public void Right()
    {
        if (InputManager.Instance)
            InputManager.Instance.TriggerMove(Vector2.right);
    }

    public void SkipDialogue()
    {
        if (InputManager.Instance)
            InputManager.Instance.TriggerSkipDialogue();
    }

}
