using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlVolData : MonoBehaviour
{
    public States.GameGenre genre;
    public Vector3 size;
    public float platformerZ;
    // Vector3(113, 67, 67) for testplatformer
    //
    private void Start()
    {
        if (genre == States.GameGenre.Platformer)
        {
            
        }
        transform.localScale = size;
    }
}
