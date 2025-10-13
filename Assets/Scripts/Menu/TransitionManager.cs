using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    //Singleton 
    public static TransitionManager Instance { get; private set; }

    [SerializeField] private GameObject _transitionUI;
    [SerializeField] private float _transitionTime = 1f;
    private Animator _animator;

    public string nextScene = "";
    private AdaptiveMusicLayering _musicManager;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        _animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        _musicManager = AdaptiveMusicLayering.Instance;
        if (_musicManager == null)
        {
            Debug.LogWarning("AdaptiveMusicLayering no encontrado. Las transiciones de música no funcionarán.");
        }
    }
    public void LoadNextScene()
    {
        //is called from the next zone script
        if (_animator)
            _animator.SetTrigger("StartTransition");

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1; 
        StartCoroutine(SceneLoad(nextSceneIndex));
    }
    public IEnumerator SceneLoad( int sceneIndex)
    {
        
        yield return new WaitForSeconds(_transitionTime);
        SceneManager.LoadScene(sceneIndex);// a index to always pass them in order
    }
    public void ChangeLevel() // UNICO QUE CAMBIE CON FADE DE MUSICA REVISAR O BORRAR EL RESTO DE METODOS
    {
            _animator.SetTrigger("StartTransition");
        StartCoroutine(SceneLoadByNameWithMusicFade(nextScene));
    }
    public IEnumerator SceneLoad()
    {
        yield return new WaitForSeconds(_transitionTime);
        SceneManager.LoadScene(nextScene);
    }
      private IEnumerator SceneLoadByNameWithMusicFade(string sceneName)
    
    {
        //  FADE OUT DE LA MÚSICA 
        float musicFadeDuration = 0f;
        if (_musicManager != null)
        {
            _musicManager.FadeOutMusicBeforeSceneChange();
            musicFadeDuration = _musicManager.GetFadeDuration();
        }

        // Espera la duración del Fade Out de la música para que termine de silenciarse.
        yield return new WaitForSeconds(musicFadeDuration);

        // Espera el tiempo restante de la animacion o musica
        float requiredWaitTime = Mathf.Max(_transitionTime, musicFadeDuration);


        yield return new WaitForSeconds(requiredWaitTime);
        SceneManager.LoadScene(sceneName);
    }

    public void FadeIn() //sobrecarga para que no me cambie de escena, solo quiero la pantalla en negro para los tps
    {
        if(_animator)
            _animator.SetTrigger("StartTransition");
    }
    public void PlayBlackScreen()
    {
        if (_animator)
            _animator.Play("FadeInanim");
    }

}
