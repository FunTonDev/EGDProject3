using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Menu Variables")]
    public int backgroundIndex;

    [Header("Game Variables")]
    public bool paused;

    [Header("Cinematic Variables")]
    public float duration;

    private void Start()
    {
        backgroundIndex = 0;
        paused = false;
        duration = 0.0f;
}
}
