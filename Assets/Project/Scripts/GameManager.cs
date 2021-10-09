using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Menu Variables")]
    public int backgroundIndex;

    [Header("Game Variables")]
    public bool paused;
    public string gameMode;

    [Header("Cinematic Variables")]
    public float duration;

    private void Start()
    {
        backgroundIndex = 0;
        paused = false;
        //Check current scene or stage manager to check for type of game stage
        gameMode = "Platformer";
        duration = 0.0f;
}
}
