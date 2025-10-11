using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; //Cinemachine Library

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }

    [SerializeField] private GameObject[] characters;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private AudioClip _changeSFX;
    private int _currentIndex = 0;

    //Referencias a la zipline
    private ArmLineController _activeZipline = null; //referencia global a la zipline activa, para que el state de Nina pueda acceder a ella
    public bool IsOnZipline { get; set; } = false; //para que el inputManager no permita cambiar de personaje si alguno esta en una zipline  [Header("Cinematica")]
    public PlayCinematic playCinematic;

    void Awake()
    {
        Debug.Log("CharacterManager: Awake() ejecutado");
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevents duplicate
            return;
        }

        Instance = this;
        ActivateCharacter(_currentIndex);
    }

    private void OnEnable()
    {
        if (InputManager.Instance != null )
            InputManager.Instance.ChangeCharacterPressed += OnChangeCharacter;
        else
            Debug.LogError("No InputManager found");
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.ChangeCharacterPressed -= OnChangeCharacter;
    }

    private void OnChangeCharacter()
    {
        Debug.Log("Change Character");
        if (GameStateManager.Instance.IsGamePaused()) return;

       if (IsOnZipline)
        {
            Debug.Log("Cannot change character while on a zipline."); //esto deberia arreglarse con lo de abajo dsps testear
            return;
        }

        foreach (var character in characters)
        {
            var stateHolder = character.GetComponent<IHasStateMachine>();
            if (stateHolder == null) continue;

            IState current = stateHolder.CurrentState;
            string stateName = current.GetType().Name; //tuve que ponerle esto porque soy un navo e hice 3 idlestate con nombres dif en vez de una parametrizado fede si lees esto perdon :C
            if ( !stateName.Contains("IdleState") && !stateName.Contains("MoveState") && !stateName.Contains("Zipline"))
            {
                Debug.Log("Cannot change character unless all characters are in Idle or Move state. Current character: " + character.name + " is in state: " + current.GetType().Name);
                return; // bloquea cambio de personaje si no estan en idle o Move
            }
        }
        SFXManager.Instance.PlaySFX(_changeSFX);
        _currentIndex = (_currentIndex + 1) % characters.Length;
        ActivateCharacter(_currentIndex);
    }


    void ActivateCharacter(int index)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            IControllable control = characters[i].GetComponent<IControllable>();
            if (control != null)
            {
               // Debug.Log("ActivateCharacter: " + characters[i].name);
               control.SetControl(i == index);
               
              // control.SetMovementEnabled(i == index);
                //This is equal to true, only for the character that is at the index in this for loop,
                //all the others are set to false so they cannot move due to their Behavior
            }
            if (_virtualCamera != null)
            {
                _virtualCamera.Follow = characters[index].transform;
                //For the VC to follow the character that is in control within the cycle
            }
        }

    }
   public void TeleportAllToCurrent()
    {
        Transform targetPosition = characters[_currentIndex].transform;
        int changePosition = 0;
        for (int i = 0; i < characters.Length; i++)
        {
            if (i != _currentIndex && changePosition < 1)
            {
              
                characters[i].transform.position = targetPosition.position + Vector3.right;
                changePosition++;
            }
            else if (i != _currentIndex)
            {
                characters[i].transform.position = targetPosition.position + Vector3.left;
            }
        }

        Debug.Log("All characters have been teleported to the active character.");
    }



    public void SetActiveZipline(ArmLineController zipline)
    {
        _activeZipline = zipline;
    }

    public ArmLineController GetActiveZipline()
    {
        return _activeZipline;
    }

    public string GetActiveCharacter()
    {
        return characters[_currentIndex].name;
    }
    public Transform GetCharacterTransform(string characterName)
    {
        foreach (var character in characters)
        {
            if (character.name == characterName)
                return character.transform;
        }
        Debug.LogWarning($"Character with name {characterName} not found in CharacterManager.");
        return null;
    }
    public IEnumerator AlignCharacters(Transform nina, Transform rex, Vector2 offsetFromRex, float speed)
    {

        Vector2 targetPos = (Vector2)rex.position + offsetFromRex;

        while (Vector2.Distance(nina.position, targetPos) > 0.05f)
        {
            nina.position = Vector2.MoveTowards(nina.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }
        nina.position = targetPos; //se fuerza la pos final de nina
        var ninaSprite = nina.GetComponentInChildren<SpriteRenderer>();
        ninaSprite.sortingOrder = 6; //para que quede delante de rex
        var rexSprite = rex.GetComponentInChildren<SpriteRenderer>();

        // Alinear las direcciones que miran ambos personajes
        Vector2 dir = rex.position - nina.position;
        bool ninaShouldFaceRight = dir.x > 0;


        if (ninaSprite != null) ninaSprite.flipX = !ninaShouldFaceRight;
        if (rexSprite != null) rexSprite.flipX = !ninaShouldFaceRight;

        // un pequeno delay antes de activar la cinematica
        yield return new WaitForSeconds(0.2f);
        playCinematic.Play();
    }
}
