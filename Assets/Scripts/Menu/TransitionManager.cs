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

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        _animator = GetComponentInChildren<Animator>();
    }
    public void LoadNextScene()
    {
        //is called from the next zone script
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1; 
        StartCoroutine(SceneLoad(nextSceneIndex));        
    }
    public IEnumerator SceneLoad( int sceneIndex)
    {
        _animator.SetTrigger("StartTransition");
        yield return new WaitForSeconds(_transitionTime);
        SceneManager.LoadScene(sceneIndex);// a index to always pass them in order
    }
}
