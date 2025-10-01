using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    //Singleton 
    public static TransitionManager Instance { get; private set; }

    [SerializeField] private GameObject _transitionUI;
    [SerializeField] private float _transitionTime = 1f;
    private Animator _animator;

    public string nextScene = "";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        _animator = GetComponentInChildren<Animator>();
    }
    public void LoadNextScene()
    {
        //is called from the next zone script
        if (_animator)
            _animator.SetTrigger("StartTransition");
        
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex - 1; 
        StartCoroutine(SceneLoad(nextSceneIndex));        
    }
    public void ChangeLevel()
    {
        if(nextScene.Length > 0)
        {
            _animator.SetTrigger("StartTransition");
            SceneManager.LoadScene(nextScene);
        }
    }
    public IEnumerator SceneLoad( int sceneIndex)
    {
        
        yield return new WaitForSeconds(_transitionTime);
        SceneManager.LoadScene(sceneIndex);// a index to always pass them in order
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
