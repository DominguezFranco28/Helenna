using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiaryNote : PlayerDetector, IActiveable
{
  public void Activeable()
    {
        SceneManager.LoadScene("MenuScreen");
    }

    public override void Effect(Collider2D collision)
    {
        Activeable();
    }
}
