using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationDoor : MonoBehaviour, IActiveable
{
    [SerializeField] private LayerMask _objectLayer;
    [SerializeField] private AudioClip _openSFX; 
    [SerializeField] private float _musicFadeDelay = 3.5f;
    [SerializeField] private float _targetVolume = 0.3f;
    private Animator _animator;
    private Collider2D _collider2D;
    private AdaptiveMusicLayering _musicLayering;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
        _musicLayering = FindAnyObjectByType<AdaptiveMusicLayering>();
        
    }

    public void Activate()
    {
        Debug.Log("ME ACTIVASTE!");
        //Sound & animation of door opening
        SFXManager.Instance.PlaySFX(_openSFX);
        _animator.SetBool("Open", true);
        Destroy(_collider2D);

        //Handling music layers as a puzzle-solving method.
        _musicLayering.FadeBaseMusicVolume(_targetVolume); 
        _musicLayering.PlayResolutionTone();
        StartCoroutine(RestoreMusicAfterDelay(_musicLayering, _musicFadeDelay)); //// call coroutine to restore music levels after a delay 
    }
    private IEnumerator RestoreMusicAfterDelay(AdaptiveMusicLayering layering, float delay)
    {
        yield return new WaitForSeconds(delay);
        layering.FadeBaseMusicVolume(1f); // original value
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & _objectLayer) != 0)
        {

            Debug.Log("OBJETO SOBRE LA PLACA");
            Activate();
            Destroy(other.gameObject);
        }
    }
}
