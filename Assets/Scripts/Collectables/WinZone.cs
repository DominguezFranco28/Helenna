using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinZone : PlayerDetector
{
    public override void Effect(Collider2D collision)
    {
        //informar al observer?2
        SceneManager.LoadScene("WinScene");
        Destroy(collision.gameObject);
    }
}
