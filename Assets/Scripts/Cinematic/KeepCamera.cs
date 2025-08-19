using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepCamera : MonoBehaviour
{
//    --solo una camara para manetner el binding de los timeline y que no se private void OnMouseEnter rompan las cinematicas

void Awake()
    {
        var cameras = FindObjectsOfType<Camera>();
        if (cameras.Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}
