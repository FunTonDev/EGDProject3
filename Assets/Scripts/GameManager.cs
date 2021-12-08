using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Components")]
    
    [Header("Variables")]
    public States.GameMode pMode;

    [Header("Menu Variables")]
    public int backgroundIndex;

    [Header("Game Variables")]
    public bool paused;
    public bool dead;
    public States.GameGenre genreMain;
    public States.GameGenre genreExtra1;
    public States.GameGenre genreExtra2;
    public AudioSource gameAudio;
    public AudioClip platformerMusic1;
    public AudioClip platformerMusic2;
    public AudioClip shooterMusic1;
    public AudioClip shooterMusic2;
    public AudioClip RPGMusic1;
    public AudioClip RPGMusic2;
    public AudioClip HubMusic;

    public SaveFile sv;

    [Header("Cinematic Variables")]
    public float duration;

    private void Start()
    {
        pMode = States.GameMode.Game;
        backgroundIndex = 0;
        paused = false;
        dead = false;
        genreMain = States.GameGenre.Platformer;
        duration = 0.0f;
        //DontDestroyOnLoad(gameObject);
        SetUpScene();
        gameAudio = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        sv = SaveManager.Load();
        if (sv.currentGenre == 1)
        {
            if (sv.inPlat)
            {
                Destroy(GameObject.Find("Cutscene1P"));
            }
            if (sv.inPlat2)
            {
                Destroy(GameObject.Find("Cutscene2P"));
            }
        }
        else if (sv.currentGenre == 2)
        {
            if (sv.inShot)
            {
                Destroy(GameObject.Find("Cutscene1S"));
            }
            if (sv.inShot1)
            {
                Destroy(GameObject.Find("Cutscene2S"));
            }
        }
        else if (sv.currentGenre == 3)
        {
            if (sv.inRPG)
            {
                Destroy(GameObject.Find("CutsceneR1"));
            }
            if (sv.inRPG2)
            {
                Destroy(GameObject.Find("CutsceneR2"));
            }
            if (sv.inRPG3)
            {
                Destroy(GameObject.Find("CutsceneR3"));
            }
        }
    }

    private void SetUpScene()
    {
        switch(pMode)
        {
            case States.GameMode.Game:
                //playCont.Initialize();
                //camCont.Initialize();
                break;
            case States.GameMode.Cinematic:
                break;
            case States.GameMode.UI:
                break;
        }
    }

    static public void SetPlayerMode()
    {

    }

    private void Update()
    {
        GameObject pla = GameObject.FindGameObjectWithTag("Player");
        if (pla.GetComponent<PlayerController>().playerSubGenre != genreMain)
        {
            genreMain = pla.GetComponent<PlayerController>().playerSubGenre;
        }
        if (genreMain == States.GameGenre.Platformer)
        {
            if (gameAudio.clip != platformerMusic1)
            {
                gameAudio.Stop();
                gameAudio.clip = platformerMusic1;
                gameAudio.Play();
            }
        }
        else if (genreMain == States.GameGenre.Shooter)
        {
            if (gameAudio.clip != shooterMusic2)
            {
                gameAudio.Stop();
                gameAudio.clip = shooterMusic2;
                gameAudio.Play();
            }
        }
        else if (genreMain == States.GameGenre.RPG)
        {
            if (gameAudio.clip != RPGMusic1)
            {
                gameAudio.Stop();
                gameAudio.clip = RPGMusic1;
                gameAudio.Play();
            }
        }
        else
        {
            if (gameAudio.clip != HubMusic)
            {
                gameAudio.Stop();
                gameAudio.clip = HubMusic;
                gameAudio.Play();
            }
        }
    }
}
