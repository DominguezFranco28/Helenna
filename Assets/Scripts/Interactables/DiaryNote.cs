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
    //Each journal note has its own text so it is passed to the Manager from its own item
    public void Activate()
    {
        SFXManager.Instance.StopLoop();
        NoteManager.Instance.ShowNote(_noteText, _nextSceneName);
        Destroy(gameObject);
    }

    public override void Effect(Collider2D collision)
    {
        Activate();
    }
}
