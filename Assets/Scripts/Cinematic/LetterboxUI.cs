using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterboxUI : MonoBehaviour
{
    public GameObject topBorder;
    public GameObject bottomBorder;

    public void ShowBorders()
    {
        topBorder.SetActive(true);
        bottomBorder.SetActive(true);
    }

    public void HideBorders()
    {
        topBorder.SetActive(false);
        bottomBorder.SetActive(false);
    }
}
