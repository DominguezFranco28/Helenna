using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextZone : PlayerDetector
{
    public override void Effect(Collider2D collision)
    {
        
        SceneManager.LoadScene("MainScene");
        
    }
}
