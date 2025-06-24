using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    //Singleton para gestion de transisiones
    public static TransitionManager Instance { get; private set; }

    [SerializeField] private GameObject _transitionUI;
    [SerializeField] private float _transitionTime = 1f;


    private Animator _animator;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        //Que comience siempre oculta la UI entre escenas.
        //_transitionUI.SetActive(false);
       
        _animator = GetComponentInChildren<Animator>();

    }
    public void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
   
        StartCoroutine(SceneLoad(nextSceneIndex));
        
    }
    public IEnumerator SceneLoad( int sceneIndex)
    {
        _animator.SetTrigger("StartTransition");
        yield return new WaitForSeconds(_transitionTime);
        SceneManager.LoadScene(sceneIndex);//para pasarlas siempre en orden
        

    }
}
