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
    public States.GameGenre genreMain;
    public States.GameGenre genreExtra1;
    public States.GameGenre genreExtra2;

    [Header("Cinematic Variables")]
    public float duration;

    private void Start()
    {
        backgroundIndex = 0;
        paused = false;
        genreMain = States.GameGenre.Platformer;
        duration = 0.0f;
    }
}
