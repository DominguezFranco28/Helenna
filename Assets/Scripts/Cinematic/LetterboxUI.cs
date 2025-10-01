using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterboxUI : MonoBehaviour
{
    public GameObject topBorder;
    public GameObject bottomBorder;

    public void ShowBorders()
    {
        if (topBorder)
            topBorder.SetActive(true);
        if (bottomBorder)
            bottomBorder.SetActive(true);
    }

    public void HideBorders()
    {
        if(topBorder)
            topBorder.SetActive(false);
        if(bottomBorder)
            bottomBorder.SetActive(false);
    }
}
