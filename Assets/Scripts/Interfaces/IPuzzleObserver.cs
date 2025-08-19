using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPuzzleObserver 
{
    void OnPuzzleEvent(int delta);
    void PuzzleSolved();
}
