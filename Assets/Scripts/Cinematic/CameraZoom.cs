using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    [SerializeField] private TimeLineTrigger _timeLineTrigger;
    public float targetZoom = 5f;
    public float zoomSpeed = 2f;
    private float originalZoom;
    private bool zooming = false;
    private bool resetting = false;
    void Start()
    {
        // guardo el zoom original de la vc
        originalZoom = virtualCamera.m_Lens.OrthographicSize;
    }

    public void StartZoom()
    {
        zooming = true;
        resetting = false;
    }

    void Update()
    {
        if (zooming)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, targetZoom, Time.deltaTime * zoomSpeed);
            _timeLineTrigger.PlayTimeline();
            if (Mathf.Abs(virtualCamera.m_Lens.OrthographicSize - targetZoom) < 0.05f)
            {
                virtualCamera.m_Lens.OrthographicSize = targetZoom;
                zooming = false;
               
            }
        }
        else if (resetting)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize,originalZoom,Time.deltaTime * zoomSpeed);

            if (Mathf.Abs(virtualCamera.m_Lens.OrthographicSize - originalZoom) < 0.05f)
            {
                virtualCamera.m_Lens.OrthographicSize = originalZoom;
                resetting = false;
            }
        }
    }

    public void ResetZoom()
    {
        resetting = true;
        zooming = false;

    }
}