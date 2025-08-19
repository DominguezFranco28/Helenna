using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineCameraFix : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private Camera mainCamera;

    void Start()
    {
        if (director != null && mainCamera != null)
        {
            // El track de la cámara debe tener como key el nombre exacto en el timeline
            director.SetGenericBinding(
                director.playableAsset.outputs.GetEnumerator().Current.sourceObject,
                mainCamera
            );
        }
    }
}
