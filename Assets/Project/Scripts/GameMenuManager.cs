using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameManager gameMan;
    public GameObject pauseMenu;
    public GameObject optionsMenu;

    [Header("Variables")]
    public int pauseIndex;
    public int optionsIndex;
    public int currentSection;

    private GameObject[] menuSections;

    /*============================================================================
     * DEFAULT UNITY METHODS
     ============================================================================*/
    private void Start()
    {
        gameMan = GameObject.Find("[MANAGER]").GetComponent<GameManager>();
        menuSections = new GameObject[] { pauseMenu, optionsMenu };
        Cursor.visible = false;
    }

    /*============================================================================
     * GAMEPLAY UPDATE METHODS
     ============================================================================*/
    public void TogglePauseMenu()
    {
        gameMan.paused = !gameMan.paused;
        Time.timeScale = gameMan.paused ? 0 : 1;
        pauseMenu.SetActive(gameMan.paused);
        Cursor.visible = gameMan.paused;
    }

    public void SwitchMenuSection(int i)
    {
        menuSections[currentSection].SetActive(false);
        currentSection = i;
        menuSections[currentSection].SetActive(true);
    }

    /*============================================================================
     * MISC METHODS
     ============================================================================*/
    [ContextMenu("Reset to Default")]
    private void SetDefaultValues()
    {
        pauseIndex = 0;
        optionsIndex = 0;
        currentSection = 0;
    }
}
