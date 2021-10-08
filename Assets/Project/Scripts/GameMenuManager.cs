using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameManager gameMan;

    public GameObject pauseMenu;

    [Header("Variables")]
    public int pauseMenuIndex;

    private void Start()
    {
        gameMan = GameObject.Find("[MANAGER]").GetComponent<GameManager>();
        Cursor.visible = false;
    }

    public void ToggleMainMenu()
    {
        gameMan.paused = !gameMan.paused;
        Time.timeScale = gameMan.paused ? 0 : 1;
        pauseMenu.SetActive(gameMan.paused);
        Cursor.visible = gameMan.paused;
    }
}
