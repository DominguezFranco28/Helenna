using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildTriggerDetector : MonoBehaviour
{
    public enum DetectorType { Climb, Zipline, Lever, Character }

    [Header("Tipo de detector")]
    [SerializeField] private DetectorType _detectorType;
    public DetectorType Type => _detectorType; //expongo el tipo de detector para acceder desde el playerbehaviour y asigar el detector correspondiente a cada objeto

    private Collider2D _internalClimbableCollider;
    private Collider2D _internalLeverCollider;
    private bool _internalCanClimb = false;
    private bool _internalCanUseZipline = false;
    private bool _internalCanActivateLever = false;
    private bool _internalCanPet = false;
    private bool _useStartPoint;

    public Collider2D Climbable => _internalClimbableCollider;
    public Collider2D LeverCollider => _internalLeverCollider;
    public bool CanClimb => _internalCanClimb;
    public bool CanUseZipline { get => _internalCanUseZipline; set => _internalCanUseZipline = value; }
    public bool CanActivate { get => _internalCanActivateLever; set => _internalCanActivateLever = value; }
    public bool CanPet { get => _internalCanPet; set => _internalCanPet = value; }

    public Vector2 GetEntryPoint(Vector2 playerPosition, ArmLineController zipline)
    {
        float distStart = Vector2.Distance(playerPosition, zipline.StartPoint);
        float distEnd = Vector2.Distance(playerPosition, zipline.EndPoint);

        // Si está más cerca del StartPoint, se mueve hacia EndPoint
        _useStartPoint = distStart < distEnd;
        return _useStartPoint ? zipline.StartPoint : zipline.EndPoint;
    }

    public Vector2 GetTargetPoint(ArmLineController zipline)
    {
        return _useStartPoint ? zipline.EndPoint : zipline.StartPoint;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (_detectorType)
        {
            case DetectorType.Climb:
                if (collision.CompareTag("Climbable"))
                {
                    _internalCanClimb = true;
                    _internalClimbableCollider = collision;
                }
                break;

            case DetectorType.Lever:
                if (collision.CompareTag("Lever"))
                {
                    _internalCanActivateLever = true;
                    _internalLeverCollider = collision;
                }
                break;

            case DetectorType.Zipline:
                // zipline no se activa al entrar, solo en Stay
                break;

            case DetectorType.Character:
                if (collision.CompareTag("DogPlayer"))
                {
                    _internalCanPet = true;
                    Debug.Log("Child can pet the dog now.");

                }
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (_detectorType)
        {
            case DetectorType.Climb:
                if (collision.CompareTag("Climbable"))
                {
                    _internalCanClimb = false;
                    _internalClimbableCollider = null;
                }
                break;

            case DetectorType.Lever:
                if (collision.CompareTag("Lever"))
                {
                    _internalCanActivateLever = false;
                    _internalLeverCollider = null;
                }
                break;

            case DetectorType.Zipline:
                if (collision.CompareTag("Zipline"))
                {
                    ResetZipline();
                }
                break;

            case DetectorType.Character:

            if (collision.CompareTag("DogPlayer"))
            {
                _internalCanPet = false;
            }
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision) //logica para ziplinen diferente al resto de colisiones, interactua mucho co el armlinecontroller de Harold
    {
        if (_detectorType == DetectorType.Zipline && collision.CompareTag("Zipline"))
        {
            ArmLineController zipline = collision.GetComponent<ArmLineController>();
            if (zipline == null) return;

            Collider2D edge = collision.GetComponent<Collider2D>();
            Vector2 closestPoint = edge.ClosestPoint(transform.position);
            float distStart = Vector2.Distance(closestPoint, zipline.StartPoint);
            float distEnd = Vector2.Distance(closestPoint, zipline.EndPoint);

            if (distStart <= 1.5f || distEnd <= 1.5f)
            {
                OnUseZipline();
            }
        }

        if (_detectorType == DetectorType.Climb && collision.CompareTag("Climbable"))
        {
            _internalCanClimb = true;
            _internalClimbableCollider = collision;
        }
    }


    public void OnUseZipline()
    {
        _internalCanUseZipline = true;
    }

    public void ResetZipline()
    {
        _internalCanUseZipline = false;
    }
}
