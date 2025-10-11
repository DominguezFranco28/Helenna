using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignCharacters : MonoBehaviour
{
    public List<Transform> characters; //asignar los personajes en el inspector

    public void Align(List<Transform> characters)
    {
        if (characters.Count < 2) return; //necesito al menos 2 personajes para alinear
        Vector3 midpoint = Vector3.zero;
        foreach (var character in characters)
        {
            midpoint += character.position;
        }
        midpoint /= characters.Count;
        foreach (var character in characters)
        {
            character.position = new Vector3(midpoint.x, character.position.y, midpoint.z);
        }
    }
}
