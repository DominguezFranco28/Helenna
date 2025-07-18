using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePulse : MonoBehaviour
{

        [SerializeField] private float _scaleSpeed = 2f;
        [SerializeField] private float _scaleAmount = 0.2f;
        private Vector3 _initialScale;

        void Start()
        {
            _initialScale = transform.localScale;
        }

        void Update()
        {
            float scaleFactor = 1 + Mathf.Sin(Time.time * _scaleSpeed) * _scaleAmount;
            transform.localScale = _initialScale * scaleFactor;
        }
}
