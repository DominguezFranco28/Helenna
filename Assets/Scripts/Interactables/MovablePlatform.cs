using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatform : MonoBehaviour , IMovable
{
    public bool elevatorTriggered = false;
    public Transform dismountPosition;
    [SerializeField] private Vector2 elevatedPosition;

    [SerializeField] private float _moveSmoothTime;
    [SerializeField] private float speed = 5f;       // Movement speed
    [SerializeField] private float smoothTime = 0.1f; // Smoothing factors

    public List<CharacterVerticalCollider> characters = new List<CharacterVerticalCollider>();
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
                if (onGround)
                {
                    character.SetToGroundColliders();
                    character.transform.position = transform.position;
                }
                else
                {
                    character.SetToElevatedColliders();
                    character.transform.position = dismountPosition.position;
                }
            }
            characters.Clear();
            ColliderBlip();
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

    private void ColliderBlip()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Collider2D>().enabled = true;
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

    public void TriggerElevator()
    {
        elevatorTriggered = true;
    }
}
