using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClosingGate : MonoBehaviour
{
    public float moveDuration = 2f; // (seconds)

    [SerializeField] private bool doorTriggered = false;
    [SerializeField] private Transform targetPoint;
    [SerializeField] private AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector3 _startPos;
    private Vector3 _endPos;
    private float _elapsedTime;
    private bool _isMoving = false;

    void Update()
    {
        if (doorTriggered)
        {
            doorTriggered = false;
            MoveTo(targetPoint, moveDuration);
        }

        if (_isMoving)
        {
            _elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(_elapsedTime / moveDuration);
            float easedT = easeCurve.Evaluate(t);

            transform.position = Vector3.Lerp(_startPos, _endPos, easedT);

            if (t >= 1f) _isMoving = false;
        }

    }

    public void MoveTo(Transform newTarget, float duration)
    {
        _startPos = transform.position;
        _endPos = newTarget.position;
        moveDuration = duration;
        _elapsedTime = 0f;
        _isMoving = true;

    }
}
