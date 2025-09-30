using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class MovablePlatform : MonoBehaviour , IMovable
{
    public bool elevatorTriggered = false;
    public Transform dismountPosition;
    [SerializeField] private Vector2 elevatedPosition;
    [SerializeField] private float usableWidth = 2.0f;  // ancho util de la plataforma para posisionar a los pjs
    [SerializeField] private float _moveSmoothTime;
    [SerializeField] private float speed = 5f;       // Movement speed
    [SerializeField] private float smoothTime = 0.1f; // Smoothing factors

    public List<CharacterVerticalCollider> characters = new List<CharacterVerticalCollider>();
    public List<GameObject> charactersTransform = new List<GameObject>();
    private Vector2 groundPosition;
    private Vector2 _target;
    private Vector2 _velocity = Vector2.zero;
    private bool onGround = true;
    private bool elevatorMoving = false;

    public int groundSpriteLayer = 2;
    public int elevatedSpriteLayer = 5;

    private void Start()
    {
        groundPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (elevatorTriggered)
        {
            elevatorTriggered = false;
            elevatorMoving = true;
            RandomPosInPlatform();
            if (onGround)
            {
                //Go Up
                onGround = false;
                _target = elevatedPosition;
            }
            else
            {
                //Go Down
                onGround = true;
                _target = groundPosition;
            }
        }

        if (elevatorMoving)
        {
            MoveTo(_target);
        }

    }

    public void MoveTo(Vector2 targetPoint)
    {
        if (targetPoint == null) return;
        Vector2 smoothPos = Vector2.SmoothDamp(transform.position,targetPoint,ref _velocity,smoothTime,speed);
        
        transform.position = smoothPos;
        foreach (CharacterVerticalCollider character in characters)
        {
            var rb = character.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.MovePosition(transform.position);
                ColliderBlip(character.gameObject);
            }
            character.transform.SetParent(transform);
        }

        if (Vector2.Distance(transform.position, targetPoint) <= 0.1f)
        {
            elevatorMoving = false;
            transform.position = targetPoint;
          

            if (onGround)
                GetComponent<SpriteRenderer>().sortingOrder = groundSpriteLayer;
            else
                GetComponent<SpriteRenderer>().sortingOrder = elevatedSpriteLayer;
                
            foreach (CharacterVerticalCollider character in characters)
            {
                character.transform.SetParent(null); //desemparento siempre al llegar
                ColliderBlip(character.gameObject);
                if (onGround)
                {
                    character.SetToGroundColliders();
                    var rb = character.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.MovePosition(transform.position);
                    }
                    character.transform.position = RandomPosOutPlatform(transform);
                    ColliderBlip(character.gameObject);

                }
                else
                {
                    character.SetToElevatedColliders();
                    var rb = character.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.MovePosition(dismountPosition.position);
                    }
                    character.transform.position = RandomPosOutPlatform(dismountPosition);
                    ColliderBlip(character.gameObject);
                }
            }
            characters.Clear();
            RefreshCharactersInPlatform();
         //   ColliderBlip(char);
         //    ColliderBlip(this.gameObject);
        }
        
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (!elevatorMoving)
        {
            CharacterVerticalCollider character = collision.gameObject.GetComponent<CharacterVerticalCollider>();
            if (character)
            {
                if (!characters.Contains(character))
                    characters.Add(character);
            }
        }


    }

    private void ColliderBlip(GameObject gameObject)
    {
        if (elevatorMoving)
        gameObject.GetComponent<Collider2D>().enabled = false;
        else
            gameObject.GetComponent<Collider2D>().enabled = true;
    }
    private void RandomPosInPlatform()
    {
        float separation = usableWidth / (characters.Count + 1);
        for (int i = 0; i < characters.Count; i++)
        {
            float baseOffset = -usableWidth / 2 + separation * (i + 1);
            float jitter = Random.Range(-separation * 0.2f, separation * 0.2f);
            Vector3 targetPos = transform.position + new Vector3(baseOffset + jitter, +1, 0);
            characters[i].transform.position = targetPos;
        }
    }
    private Vector3 RandomPosOutPlatform(Transform outPosition)
    {
        float separation = usableWidth / (characters.Count + 1);
        float randomOffset = Random.Range(-usableWidth * 0.5f, usableWidth * 0.5f);
        float jitter = Random.Range(-separation * 0.2f, separation * 0.2f);
        Vector3 finalPos = outPosition.position + new Vector3(randomOffset, +1, 0);
        return finalPos;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!elevatorMoving)
        {
            CharacterVerticalCollider character = collision.gameObject.GetComponent<CharacterVerticalCollider>();
            if (character)
            {
                if (characters.Contains(character))
                    characters.Remove(character);
       
            }

        }
        
    }
    private void RefreshCharactersInPlatform()
    {
        Collider2D platformCollider = GetComponent<Collider2D>();
        Collider2D[] colliders = Physics2D.OverlapBoxAll(platformCollider.bounds.center, platformCollider.bounds.size, 0f);

        foreach (Collider2D col in colliders)
        {
            CharacterVerticalCollider character = col.GetComponent<CharacterVerticalCollider>();
            if (character != null && !characters.Contains(character))
            {
                characters.Add(character);
            }
        }
    }

    public void TriggerElevator()
    {
        if (!elevatorMoving)
        {
            elevatorTriggered = true;
        }
        
    }
}
