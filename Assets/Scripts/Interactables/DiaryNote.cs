using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiaryNote : PlayerDetector, IActiveable
{
    [TextArea]
    [SerializeField] private string _noteText;
    [SerializeField] private string _nextSceneName;
    public void Activate()
    {
        SFXManager.Instance.StopLoop();
        NoteManager.Instance.ShowNote(_noteText, _nextSceneName); //Cada nota de diario tiene su propio texto asi, se la pasa al Manager desde su propio item
        //SceneManager.LoadScene("MenuScreen");
        Destroy(gameObject);
    }

    public override void Effect(Collider2D collision)
    {
        Activate();
    }
}
